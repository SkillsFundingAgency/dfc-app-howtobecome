using AutoMapper;
using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.Enums;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.PatchContentTypeModels;
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

        public MessageProcessor(IMapper mapper, IHttpClientService httpClientService, IMappingService mappingService)
        {
            this.mapper = mapper;
            this.httpClientService = httpClientService;
            this.mappingService = mappingService;
        }

        public async Task<HttpStatusCode> ProcessAsync(string message, long sequenceNumber, MessageContentType messageContentType, MessageAction messageAction)
        {
            var routeName = GetMappedRouteName(messageContentType.ToString());

            switch (messageContentType)
            {
                case MessageContentType.ApprenticeshipLinks:
                case MessageContentType.CollegeLinks:
                case MessageContentType.UniversityLinks:
                    {
                        var serviceBusMessage = JsonConvert.DeserializeObject<PatchLinksServiceBusModel>(message);
                        var patchLinksModel = mapper.Map<PatchLinksModel>(serviceBusMessage);
                        patchLinksModel.RouteName = routeName;
                        patchLinksModel.MessageAction = messageAction;
                        patchLinksModel.SequenceNumber = sequenceNumber;

                        return await httpClientService.PatchAsync(patchLinksModel, "links").ConfigureAwait(false);
                    }

                case MessageContentType.ApprenticeshipRequirements:
                case MessageContentType.UniversityRequirements:
                case MessageContentType.CollegeRequirements:
                    {
                        var serviceBusMessage = JsonConvert.DeserializeObject<PatchRequirementsServiceBusModel>(message);
                        var patchRequirementsModel = mapper.Map<PatchRequirementsModel>(serviceBusMessage);
                        patchRequirementsModel.RouteName = routeName;
                        patchRequirementsModel.MessageAction = messageAction;
                        patchRequirementsModel.SequenceNumber = sequenceNumber;

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

                        return await httpClientService.PatchAsync(patchSimpleClassificationModel, "entryRequirement").ConfigureAwait(false);
                    }

                case MessageContentType.JobProfile:
                    return await ProcessFullJobProfile(message, sequenceNumber, messageAction).ConfigureAwait(false);

                default:
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
            var fullJobProfile = mappingService.MapToSegmentModel(message, sequenceNumber);

            if (messageAction == MessageAction.Deleted)
            {
                return await httpClientService.DeleteAsync(fullJobProfile.DocumentId).ConfigureAwait(false);
            }

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