using Newtonsoft.Json;
using System.Collections.Generic;

namespace DFC.App.JobProfiles.HowToBecome.FunctionalTests.Model.APIResponse
{
    public class EntryRoutes
    {
        [JsonProperty("university")]
        public University University { get; set; }

        [JsonProperty("college")]
        public College College { get; set; }

        [JsonProperty("apprenticeship")]
        public Apprenticeship Apprenticeship { get; set; }

        [JsonProperty("work")]
        public List<string> Work { get; set; }

        [JsonProperty("volunteering")]
        public List<object> Volunteering { get; set; }

        [JsonProperty("directApplication")]
        public List<object> DirectApplication { get; set; }

        [JsonProperty("otherRoutes")]
        public List<string> OtherRoutes { get; set; }
    }
}
