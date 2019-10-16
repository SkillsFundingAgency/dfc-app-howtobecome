using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using System.Collections.Generic;

namespace DFC.App.JobProfiles.HowToBecome.Data.Models.DataModels
{
    public class CommonRoutes
    {
        public RouteName RouteName { get; set; }

        public string Subject { get; set; }

        public string FurtherInformation { get; set; }

        public string EntryRequirementPreface { get; set; }

        public IList<EntryRequirement> EntryRequirements { get; set; }

        public IList<AdditionalInformation> AdditionalInformation { get; set; }
    }
}