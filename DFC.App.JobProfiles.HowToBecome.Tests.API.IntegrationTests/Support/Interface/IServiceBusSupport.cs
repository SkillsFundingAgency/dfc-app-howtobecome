using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using System;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.Interface
{
    public interface IServiceBusSupport
    {
        Message CreateServiceBusMessage(Guid messageId, byte[] messageBody, ContentType contentType, ActionType actionType, CType ctype);

        Message CreateServiceBusMessage(string messageId, byte[] messageBody, ContentType contentType, ActionType actionType, CType ctype);
    }
}
