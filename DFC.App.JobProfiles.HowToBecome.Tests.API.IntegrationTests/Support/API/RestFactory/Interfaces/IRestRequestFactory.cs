using RestSharp;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.API.RestFactory.Interfaces
{
    public interface IRestRequestFactory
    {
        IRestRequest Create(string urlSuffix = null);
    }
}