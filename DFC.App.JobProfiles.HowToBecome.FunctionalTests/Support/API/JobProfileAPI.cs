using DFC.Api.JobProfiles.IntegrationTests.Model.Support;
using DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.API;
using DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.API.RestFactory.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.IntegrationTests.Support.API
{
    public class JobProfileApi : IJobProfileApi
    {
        private readonly IRestClientFactory restClientFactory;
        private readonly IRestRequestFactory restRequestFactory;
        private readonly AppSettings appSettings;
        private readonly APISettings apiSettings;

        public JobProfileApi(IRestClientFactory restClientFactory, IRestRequestFactory restRequestFactory, AppSettings appSettings, APISettings apiSettings)
        {
            this.restClientFactory = restClientFactory;
            this.restRequestFactory = restRequestFactory;
            this.appSettings = appSettings;
            this.apiSettings = apiSettings;
        }

        public async Task<RestResponse<T>> GetById<T>(string id)
            where T : class, new()
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            var restClient = this.restClientFactory.Create(this.apiSettings.Endpoint);
            var restRequest = this.restRequestFactory.Create($"/segment/{id}/contents");

            foreach (KeyValuePair<string, string> queryParameter in this.apiSettings.QueryParameters)
            {
                restRequest.AddParameter(queryParameter.Key, queryParameter.Value);
            }

            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddHeader("Ocp-Apim-Subscription-Key", this.appSettings.APIConfig.ApimSubscriptionKey);
            restRequest.AddHeader("version", this.appSettings.APIConfig.Version);
            return await Task.Run(() => restClient.Execute<T>(restRequest)).ConfigureAwait(false);
        }
    }
}