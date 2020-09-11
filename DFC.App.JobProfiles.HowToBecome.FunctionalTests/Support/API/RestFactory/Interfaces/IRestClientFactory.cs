using RestSharp;
using System;

namespace DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.API.RestFactory.Interfaces
{
    public interface IRestClientFactory
    {
        IRestClient Create(Uri baseUrl);

        IRestClient Create(string baseUrl);
    }
}