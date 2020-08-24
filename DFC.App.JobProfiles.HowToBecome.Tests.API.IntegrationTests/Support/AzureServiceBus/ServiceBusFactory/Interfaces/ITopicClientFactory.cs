using Microsoft.Azure.ServiceBus;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.AzureServiceBus.ServiceBusFactory.Interfaces
{
    public interface ITopicClientFactory
    {
        ITopicClient Create(string connectionString);
    }
}