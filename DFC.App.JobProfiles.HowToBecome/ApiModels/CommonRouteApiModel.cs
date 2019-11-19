using System.Collections.Generic;

namespace DFC.App.JobProfiles.HowToBecome.ApiModels
{
    public class CommonRouteApiModel
    {
        public List<string> RelevantSubjects { get; set; }

        public List<string> FurtherInformation { get; set; }

        public string EntryRequirementPreface { get; set; }

        public List<string> EntryRequirements { get; set; }

        public List<string> AdditionalInformation { get; set; }
    }
}
