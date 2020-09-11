using DFC.Api.JobProfiles.IntegrationTests.Model.Support;
using DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.AzureServiceBus;
using DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.AzureServiceBus.ServiceBusFactory.Interfaces;
using FakeItEasy;
using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobProfiles.HowToBecome.TestFramework.UnitTests
{
    public class ServiceBusSupportTests
    {
        [Fact]
        public async Task SendMessageSendsMessageOnTopicClient()
        {
            // Arrange
            var message = A.Fake<Message>();
            var fakeTopicClient = A.Fake<ITopicClient>();
            var fakeTopicClientFactory = A.Fake<ITopicClientFactory>();
            A.CallTo(() => fakeTopicClientFactory.Create(A<string>.Ignored)).Returns(fakeTopicClient);
            var serviceBusSupport = new ServiceBusSupport(fakeTopicClientFactory, new AppSettings());

            // Act
            await serviceBusSupport.SendMessage(message).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeTopicClient.SendAsync(message)).MustHaveHappenedOnceExactly();
        }
    }
}
