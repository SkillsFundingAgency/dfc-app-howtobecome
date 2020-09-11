using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.API.RestFactory.Interfaces;
using RestSharp;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.API.RestFactory
{
    internal class RestRequestFactory : IRestRequestFactory
    {
        public IRestRequest Create(string urlSuffix = null)
        {
            return new RestRequest(urlSuffix);
        }
    }
}