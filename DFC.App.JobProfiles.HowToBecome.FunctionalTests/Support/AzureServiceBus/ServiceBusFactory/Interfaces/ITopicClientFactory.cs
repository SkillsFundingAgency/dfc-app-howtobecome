using Microsoft.Azure.ServiceBus;

namespace DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.AzureServiceBus.ServiceBusFactory.Interfaces
{
    public interface ITopicClientFactory
    {
        ITopicClient Create(string connectionString);
    }
}