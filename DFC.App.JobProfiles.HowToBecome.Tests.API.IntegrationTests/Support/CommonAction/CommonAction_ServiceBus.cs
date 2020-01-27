using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.Interface;
using System;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support
{
    internal partial class CommonAction : IServiceBusSupport
    {
        public Message CreateServiceBusMessage(Guid messageId, byte[] messageBody, ContentType contentType, ActionType actionType, CType ctype)
        {
            Message message = new Message();
            message.ContentType = GetDescription(contentType);
            message.Body = messageBody;
            message.CorrelationId = Guid.NewGuid().ToString();
            message.Label = "Automated message";
            message.MessageId = messageId.ToString();
            message.UserProperties.Add("Id", messageId);
            message.UserProperties.Add("ActionType", actionType.ToString());
            message.UserProperties.Add("CType", ctype.ToString());
            return message;
        }
    }
}
