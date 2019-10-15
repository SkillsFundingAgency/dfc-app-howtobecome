using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.Enums;
using DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Services;
using DFC.Functions.DI.Standard.Attributes;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Functions
{
    public class SitefinityMessageHandler
    {
        [FunctionName("SitefinityMessageHandler")]
        public async Task Run(
            [ServiceBusTrigger("%cms-messages-topic%", "%cms-messages-subscription%", Connection = "service-bus-connection-string")] Message sitefinityMessage,
            [Inject] IMessageProcessor messageProcessor,
            [Inject] ILogger log)
        {
            if (sitefinityMessage == null)
            {
                throw new ArgumentNullException(nameof(sitefinityMessage));
            }

            sitefinityMessage.UserProperties.TryGetValue("EventType", out var eventType);
            sitefinityMessage.UserProperties.TryGetValue("CType", out var contentType);
            sitefinityMessage.UserProperties.TryGetValue("Id", out var messageContentId);

            // loggger should allow setting up correlation id and should be picked up from message
            log.LogInformation($"{nameof(SitefinityMessageHandler)}: Received message action '{eventType}' for type '{contentType}' with Id: '{messageContentId}': Correlation id {sitefinityMessage.CorrelationId}");

            var message = Encoding.UTF8.GetString(sitefinityMessage?.Body);
            var parsedEventType = Enum.Parse<MessageAction>(eventType.ToString());

            await messageProcessor.ProcessAsync(message, sitefinityMessage.SystemProperties.SequenceNumber, contentType?.ToString(), parsedEventType).ConfigureAwait(false);
        }
    }
}