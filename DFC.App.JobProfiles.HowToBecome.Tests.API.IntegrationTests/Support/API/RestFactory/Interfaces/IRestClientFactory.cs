using RestSharp;
using System;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.API.RestFactory.Interfaces
{
    public interface IRestClientFactory
    {
        IRestClient Create(Uri baseUrl);

        IRestClient Create(string baseUrl);
    }
}