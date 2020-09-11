using Newtonsoft.Json;
using System.Collections.Generic;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.APIResponse
{
    public class University
    {
        [JsonProperty("relevantSubjects")]
        public List<string> RelevantSubjects { get; set; }

        [JsonProperty("furtherInformation")]
        public List<string> FurtherInformation { get; set; }

        [JsonProperty("entryRequirementPreface")]
        public string EntryRequirementPreface { get; set; }

        [JsonProperty("entryRequirements")]
        public List<string> EntryRequirements { get; set; }

        [JsonProperty("additionalInformation")]
        public List<string> AdditionalInformation { get; set; }
    }
}
