using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.Common.AzureServiceBusSupport
{
    public class Topic
    {
        public Topic(string endpoint)
        {
            ServiceBusConnectionStringBuilder connectionString = new ServiceBusConnectionStringBuilder(endpoint);
            this.TopicClient = new TopicClient(connectionString);
        }

        private TopicClient TopicClient { get; set; }

        public async Task SendAsync(Message message)
        {
            await this.TopicClient.SendAsync(message).ConfigureAwait(true);
        }
    }
}
