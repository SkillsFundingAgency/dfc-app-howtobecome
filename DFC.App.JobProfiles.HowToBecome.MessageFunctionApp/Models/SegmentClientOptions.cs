using System;

namespace DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Models
{
    public class SegmentClientOptions
    {
        public Uri BaseAddress { get; set; }

        public string GetEndpoint { get; set; }

        public string PatchLinksEndpoint { get; set; }

        public string PatchRequirementsEndpoint { get; set; }

        public string PatchSimpleClassificationEndpoint { get; set; }

        public string PostEndpoint { get; set; }

        public string PutEndpoint { get; set; }

        public string DeleteEndpoint { get; set; }

        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 30);         // default to 30 seconds
    }
}