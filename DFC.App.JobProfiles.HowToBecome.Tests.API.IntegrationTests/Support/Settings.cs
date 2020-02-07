using System;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support
{
    internal class Settings
    {
        internal static TimeSpan GracePeriod { get; set; }

        internal class ServiceBusConfig
        {
            internal static string Endpoint { get; set; }
        }

        internal class APIConfig
        {
            internal static string Version { get; set; }

            internal static string ApimSubscriptionKey { get; set; }

            internal static string EndpointBaseUrl { get; set; }
        }
    }
}
