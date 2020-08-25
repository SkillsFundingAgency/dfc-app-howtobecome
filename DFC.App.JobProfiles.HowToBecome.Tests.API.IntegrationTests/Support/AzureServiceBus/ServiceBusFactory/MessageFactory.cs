using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.AzureServiceBus.ServiceBusFactory.Interfaces;
using Microsoft.Azure.ServiceBus;
using System;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.AzureServiceBus.ServiceBusFactory
{
    public class MessageFactory : IMessageFactory
    {
        public Message Create(string messageId, byte[] messageBody, string actionType, string contentType)
        {
            var message = new Message
            {
                ContentType = contentType,
                Body = messageBody,
                CorrelationId = Guid.NewGuid().ToString(),
                Label = "Automated message",
                MessageId = messageId,
            };

            message.UserProperties.Add("ActionType", actionType);
            message.UserProperties.Add("CType", contentType);
            message.UserProperties.Add("Id", messageId);
            return message;
        }
    }
}