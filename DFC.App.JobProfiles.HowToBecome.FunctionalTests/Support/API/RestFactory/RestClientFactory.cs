using DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.API.RestFactory.Interfaces;
using RestSharp;
using System;

namespace DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.API.RestFactory
{
    internal class RestClientFactory : IRestClientFactory
    {
        public IRestClient Create(Uri baseUrl)
        {
            return new RestClient(baseUrl);
        }

        public IRestClient Create(string baseUrl)
        {
            return new RestClient(baseUrl);
        }
    }
}