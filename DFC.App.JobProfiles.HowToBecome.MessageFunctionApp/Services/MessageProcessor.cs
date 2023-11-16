using AutoMapper;
using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.Enums;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.PatchContentTypeModels;
using Microsoft.Azure.Amqp;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Services
{
    public class MessageProcessor : IMessageProcessor
    {
        private readonly IMapper mapper;
        private readonly IHttpClientService httpClientService;
        private readonly IMappingService mappingService;
        private readonly ILogger<MessageProcessor> logger;

        public MessageProcessor(IMapper mapper, IHttpClientService httpClientService, IMappingService mappingService, ILogger<MessageProcessor> logger)
        {
            this.mapper = mapper;
            this.httpClientService = httpClientService;
            this.mappingService = mappingService;
            this.logger = logger;
        }

        public async Task<HttpStatusCode> ProcessAsync(string message, long sequenceNumber, MessageContentType messageContentType, MessageAction messageAction)
        {
            var routeName = GetMappedRouteName(messageContentType.ToString());
            logger.LogInformation($"MessageProcessor ProcessAsync message{message} sequenceNumber {sequenceNumber} ");
            switch (messageContentType)
            {
                case MessageContentType.ApprenticeshipLink:
                case MessageContentType.CollegeLink:
                case MessageContentType.UniversityLink:
                    {
                        var serviceBusMessage = JsonConvert.DeserializeObject<PatchLinksServiceBusModel>(message);
                        var patchLinksModel = mapper.Map<PatchLinksModel>(serviceBusMessage);
                        patchLinksModel.RouteName = routeName;
                        patchLinksModel.MessageAction = messageAction;
                        patchLinksModel.SequenceNumber = sequenceNumber;
                        logger.LogInformation($"MessageProcessor ProcessAsync message{message} sequenceNumber {sequenceNumber} contenttype UniversityLink ");
                        return await httpClientService.PatchAsync(patchLinksModel, "links").ConfigureAwait(false);
                    }

                case MessageContentType.ApprenticeshipRequirement:
                case MessageContentType.UniversityRequirement:
                case MessageContentType.CollegeRequirement:
                    {
                        var serviceBusMessage = JsonConvert.DeserializeObject<PatchRequirementsServiceBusModel>(message);
                        var patchRequirementsModel = mapper.Map<PatchRequirementsModel>(serviceBusMessage);
                        patchRequirementsModel.RouteName = routeName;
                        patchRequirementsModel.MessageAction = messageAction;
                        patchRequirementsModel.SequenceNumber = sequenceNumber;
                        logger.LogInformation($"MessageProcessor ProcessAsync message{message} sequenceNumber {sequenceNumber} contenttype ApprenticeshipRequirement/UniversityRequirement/CollegeRequirement ");
                        return await httpClientService.PatchAsync(patchRequirementsModel, "requirements").ConfigureAwait(false);
                    }

                case MessageContentType.ApprenticeshipEntryRequirements:
                case MessageContentType.UniversityEntryRequirements:
                case MessageContentType.CollegeEntryRequirements:
                    {
                        var serviceBusMessage = JsonConvert.DeserializeObject<PatchSimpleClassificationServiceBusModel>(message);
                        var patchSimpleClassificationModel = mapper.Map<PatchSimpleClassificationModel>(serviceBusMessage);
                        patchSimpleClassificationModel.RouteName = routeName;
                        patchSimpleClassificationModel.MessageAction = messageAction;
                        patchSimpleClassificationModel.SequenceNumber = sequenceNumber;
                        logger.LogInformation($"MessageProcessor ProcessAsync message{message} sequenceNumber {sequenceNumber} contenttype ApprenticeshipEntryRequirements/UniversityEntryRequirements/CollegeEntryRequirements ");

                        return await httpClientService.PatchAsync(patchSimpleClassificationModel, "entryRequirement").ConfigureAwait(false);
                    }

                case MessageContentType.Registration:
                    {
                        var serviceBusMessage = JsonConvert.DeserializeObject<PatchRegistrationsServiceBusModel>(message);
                        var patchRegistrationsModel = mapper.Map<PatchRegistrationModel>(serviceBusMessage);
                        patchRegistrationsModel.RouteName = routeName;
                        patchRegistrationsModel.MessageAction = messageAction;
                        patchRegistrationsModel.SequenceNumber = sequenceNumber;
                        logger.LogInformation($"MessageProcessor ProcessAsync message{message} sequenceNumber {sequenceNumber} contenttype Registration");

                        return await httpClientService.PatchAsync(patchRegistrationsModel, "registration").ConfigureAwait(false);
                    }

                case MessageContentType.RealStory:
                    {
                        var serviceBusMessage = JsonConvert.DeserializeObject<PatchRealStoryServiceBusModel>(message);
                        var patchRealStoryModel = mapper.Map<PatchRealStoryModel>(serviceBusMessage);
                        patchRealStoryModel.RouteName = routeName;
                        patchRealStoryModel.MessageAction = messageAction;
                        patchRealStoryModel.SequenceNumber = sequenceNumber;
                        logger.LogInformation($"MessageProcessor ProcessAsync message{message} sequenceNumber {sequenceNumber} contenttype RealStory");

                        return await httpClientService.PatchAsync(patchRealStoryModel, "realStory").ConfigureAwait(false);
                    }

                case MessageContentType.JobProfile:
                    logger.LogInformation($"MessageProcessor ProcessAsync message{message} sequenceNumber {sequenceNumber} contenttype JobProfile");
                    return await ProcessFullJobProfile(message, sequenceNumber, messageAction).ConfigureAwait(false);

                default:
                    logger.LogInformation($"MessageProcessor ProcessAsync message{message} sequenceNumber {sequenceNumber} Unexpected sitefinity content type {messageContentType}");
                    throw new ArgumentOutOfRangeException(nameof(messageContentType), $"Unexpected sitefinity content type '{messageContentType}'");
            }
        }

        private static RouteName GetMappedRouteName(string sitefinityContentType)
        {
           switch (sitefinityContentType)
            {
                case var _ when sitefinityContentType.Contains(nameof(RouteName.Apprenticeship), StringComparison.OrdinalIgnoreCase):
                    return RouteName.Apprenticeship;

                case var _ when sitefinityContentType.Contains(nameof(RouteName.University), StringComparison.OrdinalIgnoreCase):
                    return RouteName.University;

                case var _ when sitefinityContentType.Contains(nameof(RouteName.College), StringComparison.OrdinalIgnoreCase):
                    return RouteName.College;

                default:
                    return RouteName.Unknown;
            }
        }

        private async Task<HttpStatusCode> ProcessFullJobProfile(string message, long sequenceNumber, MessageAction messageAction)
        {
            logger.LogInformation($"ProcessFullJobProfile message {message} ");
            var fullJobProfile = mappingService.MapToSegmentModel(message, sequenceNumber);

            if (messageAction == MessageAction.Deleted)
            {
                return await httpClientService.DeleteAsync(fullJobProfile.DocumentId).ConfigureAwait(false);
            }

            logger.LogInformation($"ProcessFullJobProfile message {message} ");

            return await SendMessageData(fullJobProfile).ConfigureAwait(false);
        }

        private async Task<HttpStatusCode> SendMessageData(HowToBecomeSegmentModel fullModel)
        {
            var result = await httpClientService.PutFullJobProfileAsync(fullModel).ConfigureAwait(false);
            if (result == HttpStatusCode.NotFound)
            {
                return await httpClientService.PostFullJobProfileAsync(fullModel).ConfigureAwait(false);
            }
      
            return result;
        }
    }
}