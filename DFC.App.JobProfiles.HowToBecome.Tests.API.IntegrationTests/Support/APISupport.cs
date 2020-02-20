using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.RestFactory;
using RestSharp;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support
{
    internal class APISupport : IAPISupport
    {
        public APISupport(IRestClientFactory restClientFactory, IRestRequestFactory restRequestFactory, string apimSubscriptionKey)
        {
            this.RestClientFactory = restClientFactory;
            this.RestRequestFactory = restRequestFactory;
            this.ApimSubscriptionKey = apimSubscriptionKey;
        }

        private IRestClientFactory RestClientFactory { get; set; }

        private IRestRequestFactory RestRequestFactory { get; set; }

        private string ApimSubscriptionKey { get; set; }

        public async Task<IRestResponse> GetByJobProfileId(Guid id)
        {
            IRestClient restClient = this.RestClientFactory.Create(Settings.APIConfig.EndpointBaseUrl);
            IRestRequest restRequest = this.RestRequestFactory.Create(string.Format(CultureInfo.CurrentCulture, "segment/{0}/contents", id.ToString()), Method.GET);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddHeader("version", Settings.APIConfig.Version);
            restRequest.AddHeader("Ocp-Apim-Subscription-Key", this.ApimSubscriptionKey);
            return await restClient.ExecuteAsync(restRequest, Method.GET).ConfigureAwait(true);
        }
    }
}