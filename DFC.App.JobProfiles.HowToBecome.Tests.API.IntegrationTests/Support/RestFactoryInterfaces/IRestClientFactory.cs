using RestSharp;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.RestFactory
{
    internal interface IRestClientFactory
    {
        RestClient Create(string baseUrl);
    }
}
