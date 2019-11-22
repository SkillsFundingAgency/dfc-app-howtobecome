using System.Net.Http;

namespace DFC.App.JobProfiles.HowToBecome.MFA.UnitTests.FakeHttpHandlers
{
    public interface IFakeHttpRequestSender
    {
        HttpResponseMessage Send(HttpRequestMessage request);
    }
}