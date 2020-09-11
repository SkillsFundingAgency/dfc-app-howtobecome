using Microsoft.Azure.ServiceBus;

namespace DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.AzureServiceBus.ServiceBusFactory.Interfaces
{
    public interface IMessageFactory
    {
        Message Create(string messageId, byte[] messageBody, string actionType, string contentType);
    }
}