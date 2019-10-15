using AutoMapper;
using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.Enums;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.PatchContentTypeModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Services
{
    public class MessageProcessor : IMessageProcessor
    {
        private static readonly string ClassFullName = typeof(MessageProcessor).FullName;

        private readonly IMapper mapper;
        private readonly IHttpClientService httpClientService;
        private readonly IMappingService mappingService;

        private readonly ILogger log;

        public MessageProcessor(IMapper mapper, IHttpClientService httpClientService, ILogger log, IMappingService mappingService)
        {
            this.mapper = mapper;
            this.httpClientService = httpClientService;
            this.log = log;
            this.mappingService = mappingService;
        }

        public async Task ProcessAsync(string message, long sequenceNumber, string sitefinityContentType, EventType eventType)
        {
            // Remove this mapping once confirmed there can be a routename added to the message
            var routeName = GetMappedRouteName(sitefinityContentType);

            switch (sitefinityContentType)
            {
                case "ApprenticeshipLinks":
                case "UniversityLinks":
                case "CollegeLinks":
                    {
                        var serviceBusMessage = JsonConvert.DeserializeObject<PatchLinksServiceBusModel>(message);
                        var patchLinksModel = mapper.Map<PatchLinksModel>(serviceBusMessage);
                        patchLinksModel.RouteName = routeName;
                        patchLinksModel.EventType = eventType;
                        patchLinksModel.SequenceNumber = sequenceNumber;

                        await httpClientService.PatchAsync(patchLinksModel, "links").ConfigureAwait(false)

                        break;
                    }

                case "ApprenticeshipRequirements":
                case "UniversityRequirements":
                case "CollegeRequirements":
                    {
                        var serviceBusMessage = JsonConvert.DeserializeObject<PatchRequirementsServiceBusModel>(message);
                        var patchRequirementsModel = mapper.Map<PatchRequirementsModel>(serviceBusMessage);
                        patchRequirementsModel.RouteName = routeName;
                        patchRequirementsModel.EventType = eventType;
                        patchRequirementsModel.SequenceNumber = sequenceNumber;

                        await httpClientService.PatchAsync(patchRequirementsModel, "requirements").ConfigureAwait(false);

                        break;
                    }

                case "ApprenticeshipEntryRequirements":
                case "UniversityEntryRequirements":
                case "CollegeEntryRequirements":
                    {
                        var serviceBusMessage = JsonConvert.DeserializeObject<PatchSimpleClassificationServiceBusModel>(message);
                        var patchSimpleClassificationModel = mapper.Map<PatchSimpleClassificationModel>(serviceBusMessage);
                        patchSimpleClassificationModel.RouteName = routeName;
                        patchSimpleClassificationModel.EventType = eventType;
                        patchSimpleClassificationModel.SequenceNumber = sequenceNumber;
                        await httpClientService.PatchAsync(patchSimpleClassificationModel, "entryRequirement").ConfigureAwait(false);

                        break;
                    }

                case "JobProfile":
                    await ProcessFullJobProfile(message, sequenceNumber, eventType).ConfigureAwait(false);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(sitefinityContentType), $"Unexpected sitefinity content type '{sitefinityContentType}'");
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

        private async Task ProcessFullJobProfile(string message, long sequenceNumber, EventType eventType)
        {
            var fullJobProfile = mappingService.MapToSegmentModel(message, sequenceNumber);

            if (eventType == EventType.Deleted)
            {
                await httpClientService.DeleteAsync(fullJobProfile.DocumentId).ConfigureAwait(false);
            }

            var result = await SendMessageData(fullJobProfile).ConfigureAwait(false);
            LogResult(result, fullJobProfile.DocumentId);
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

        private void LogResult(HttpStatusCode response, Guid jobProfileId)
        {
            switch (response)
            {
                case HttpStatusCode.OK:
                    log.LogInformation(
                        $"{ClassFullName}: JobProfile Id: {jobProfileId}: Updated segment");
                    break;

                case HttpStatusCode.Created:
                    log.LogInformation(
                        $"{ClassFullName}: JobProfile Id: {jobProfileId}: Created segment");
                    break;

                default:
                    log.LogWarning(
                        $"{ClassFullName}: JobProfile Id: {jobProfileId}: Segment not Posted: Status: {response}");
                    break;
            }
        }
    }
}