using System;

namespace DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Models
{
    public class SegmentClientOptions
    {
        public Uri BaseAddress { get; set; }

        public string GetEndpoint { get; set; } = "segment/{0}/contents";

        public string PatchLinksEndpoint { get; set; } = "segment/{0}/links";

        public string PatchRequirementsEndpoint { get; set; } = "segment/{0}/requirements";

        public string PatchSimpleClassificationEndpoint { get; set; } = "segment/{0}/simple-classification";

        public string PostEndpoint { get; set; } = "segment";

        public string PutEndpoint { get; set; } = "segment";

        public string DeleteEndpoint { get; set; } = "segment/{0}";

        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 10);         // default to 30 seconds
    }
}