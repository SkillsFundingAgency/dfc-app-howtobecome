using System;

namespace DFC.App.JobProfileOverview.Tests.IntegrationTests.API.Model.Support
{
    public class APIConfig
    {
        public string Version { get; set; }

        public string ApimSubscriptionKey { get; set; }

        public Uri EndpointBaseUrl { get; set; }
    }
}
