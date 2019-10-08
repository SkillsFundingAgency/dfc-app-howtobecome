using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.Enums;
using DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Services;
using DFC.Functions.DI.Standard.Attributes;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Functions
{
    public class ProcessSegmentEvents
    {
        [FunctionName("ProcessSegmentEvents")]
        public async Task Run(
            [ServiceBusTrigger("%post-segment-topic-name%", "%post-segment-subscription-name%", Connection = "service-bus-connection-string")] Message serviceBusMessage,
            [Inject] IMessageProcessor messageProcessor)
        {
            if (serviceBusMessage != null)
            {
                var message = Encoding.UTF8.GetString(serviceBusMessage?.Body);
                serviceBusMessage.UserProperties.TryGetValue("EventType", out var eventType); // Parse to enum values
                serviceBusMessage.UserProperties.TryGetValue("CType", out var contentType);

                var parsedEventType = Enum.Parse<EventType>(eventType.ToString());

                await messageProcessor.ProcessUpdateMessage(message, serviceBusMessage.SystemProperties.SequenceNumber, contentType?.ToString(), parsedEventType).ConfigureAwait(false);
            }
        }
    }
}