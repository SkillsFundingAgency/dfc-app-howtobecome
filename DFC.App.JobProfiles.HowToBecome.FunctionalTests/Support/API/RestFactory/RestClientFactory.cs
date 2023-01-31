using DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.API.RestFactory.Interfaces;
using RestSharp;
using System;

namespace DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.API.RestFactory
{
    internal class RestClientFactory : IRestClientFactory
    {
        public RestClient Create(Uri baseUrl)
        {
            return new RestClient(baseUrl);
        }

        public RestClient Create(string baseUrl)
        {
            return new RestClient(baseUrl);
        }
    }
}