using RestSharp;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.RestFactory
{
    internal interface IRestRequestFactory
    {
        RestRequest Create(string url, Method method);
    }
}
