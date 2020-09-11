using Newtonsoft.Json;
using System.Collections.Generic;

namespace DFC.App.JobProfiles.HowToBecome.FunctionalTests.Model.APIResponse
{
    public class HowToBecomeAPIResponse
    {
        [JsonProperty("entryRouteSummary")]
        public List<string> EntryRouteSummary { get; set; }

        [JsonProperty("entryRoutes")]
        public EntryRoutes EntryRoutes { get; set; }

        [JsonProperty("moreInformation")]
        public MoreInformation MoreInformation { get; set; }
    }
}
