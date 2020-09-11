using RestSharp;

namespace DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.API.RestFactory.Interfaces
{
    public interface IRestRequestFactory
    {
        IRestRequest Create(string urlSuffix = null);
    }
}