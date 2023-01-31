using RestSharp;
using System;

namespace DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.API.RestFactory.Interfaces
{
    public interface IRestClientFactory
    {
        RestClient Create(Uri baseUrl);

        RestClient Create(string baseUrl);
    }
}