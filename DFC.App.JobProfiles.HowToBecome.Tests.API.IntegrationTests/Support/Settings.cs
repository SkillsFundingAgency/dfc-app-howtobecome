using System;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support
{
    public class Settings
    {
        public static TimeSpan GracePeriod { get; set; }

        public class ServiceBusConfig
        {
            public static string Endpoint { get; set; }
        }

        public class APIConfig
        {
            public static string Version { get; set; }
            public static string ApimSubscriptionKey { get; set; }
            public static EndpointBaseUrl EndpointBaseUrl { get; set; } = new EndpointBaseUrl();
        }

        public class EndpointBaseUrl
        {
            public string ProfileDetail { get; set; }
            public string HowToSegment { get; set; }
        }
    }
}
