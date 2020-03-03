using Newtonsoft.Json;
using System.Collections.Generic;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.API
{
    public class MoreInformation
    {
        [JsonProperty("registrations")]
        public List<object> Registrations { get; set; }

        [JsonProperty("careerTips")]
        public List<string> CareerTips { get; set; }

        [JsonProperty("professionalAndIndustryBodies")]
        public List<string> ProfessionalAndIndustryBodies { get; set; }

        [JsonProperty("furtherInformation")]
        public List<string> FurtherInformation { get; set; }
    }
}
