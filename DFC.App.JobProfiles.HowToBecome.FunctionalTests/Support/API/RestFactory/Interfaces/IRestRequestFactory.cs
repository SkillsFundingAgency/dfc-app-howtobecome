using RestSharp;

namespace DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.API.RestFactory.Interfaces
{
    public interface IRestRequestFactory
    {
        RestRequest Create(string urlSuffix = null);
    }
}