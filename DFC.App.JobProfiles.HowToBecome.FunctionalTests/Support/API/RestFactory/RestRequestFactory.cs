using DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.API.RestFactory.Interfaces;
using RestSharp;

namespace DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.API.RestFactory
{
    internal class RestRequestFactory : IRestRequestFactory
    {
        public RestRequest Create(string urlSuffix = null)
        {
            return new RestRequest(urlSuffix);
        }
    }
}