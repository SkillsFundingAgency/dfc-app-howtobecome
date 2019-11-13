using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.Enums;
using DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Services;
using DFC.Functions.DI.Standard.Attributes;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Functions
{
    public class SitefinityMessageHandler
    {
        private static readonly string ClassFullName = typeof(SitefinityMessageHandler).FullName;

        [FunctionName("SitefinityMessageHandler")]
        public async Task Run(
            [ServiceBusTrigger("%cms-messages-topic%", "%cms-messages-subscription%", Connection = "service-bus-connection-string")] Message sitefinityMessage,
            [Inject] IMessageProcessor messageProcessor,
            ILogger log)
        {
            if (sitefinityMessage == null)
            {
                throw new ArgumentNullException(nameof(sitefinityMessage));
            }
            long sequenceNumber = sitefinityMessage.SystemProperties.SequenceNumber;

            sitefinityMessage.UserProperties.TryGetValue("ActionType", out var actionType);
            sitefinityMessage.UserProperties.TryGetValue("CType", out var contentType);
            sitefinityMessage.UserProperties.TryGetValue("Id", out var messageContentId);

            // loggger should allow setting up correlation id and should be picked up from message
            log.LogInformation($"{nameof(SitefinityMessageHandler)}: Received message action '{actionType}' for type '{contentType}' with Id: '{messageContentId}', Sequence Number: {sequenceNumber}, Correlation id {sitefinityMessage.CorrelationId}");

            var message = Encoding.UTF8.GetString(sitefinityMessage?.Body);

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message cannot be null or empty.", nameof(sitefinityMessage));
            }

            if (!Enum.TryParse<MessageAction>(actionType?.ToString(), out var messageAction))
            {
                throw new ArgumentOutOfRangeException(nameof(actionType), $"Invalid message action '{actionType}' received, should be one of '{string.Join(",", Enum.GetNames(typeof(MessageAction)))}'");
            }

            if (!Enum.TryParse<MessageContentType>(contentType?.ToString(), out var messageContentType))
            {
                throw new ArgumentOutOfRangeException(nameof(contentType), $"Invalid message content type '{contentType}' received, should be one of '{string.Join(",", Enum.GetNames(typeof(MessageContentType)))}'");
            }

            HttpStatusCode result;

            try
            {
                result = await messageProcessor.ProcessAsync(message, sequenceNumber, messageContentType, messageAction).ConfigureAwait(false);

                log.LogError($"{ClassFullName}: JobProfile Id: {messageContentId}: messageAction: {messageAction}, messageContentType: {messageContentType}, Sequence Number: {sequenceNumber}, RESULT: {result}");
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"{ClassFullName}: JobProfile Id: {messageContentId}: messageAction: {messageAction}, messageContentType: {messageContentType}, Sequence Number: {sequenceNumber}");
                throw;
            }

            switch (result)
            {
                case HttpStatusCode.OK:
                    log.LogInformation($"{ClassFullName}: JobProfile Id: {messageContentId}: Sequence Number: {sequenceNumber}, Updated segment");
                    break;

                case HttpStatusCode.Created:
                    log.LogInformation($"{ClassFullName}: JobProfile Id: {messageContentId}: Sequence Number: {sequenceNumber}, Created segment");
                    break;

                case HttpStatusCode.AlreadyReported:
                    log.LogInformation($"{ClassFullName}: JobProfile Id: {messageContentId}: Sequence Number: {sequenceNumber}, Segment previously updated");
                    break;

                default:
                    log.LogWarning($"{ClassFullName}: JobProfile Id: {messageContentId}: Sequence Number: {sequenceNumber}, Segment not Posted: Status: {result}");
                    break;
            }
        }
    }
}
