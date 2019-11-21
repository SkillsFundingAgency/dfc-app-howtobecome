using System.Collections.Generic;

namespace DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels
{
    public class SitefinityRouteEntries
    {
        public int RouteName { get; set; }

        public List<SitefinityEntryRequirement> EntryRequirements { get; set; }

        public List<SitefinityMoreInformationLinks> MoreInformationLinks { get; set; }

        public string RouteSubjects { get; set; }

        public string FurtherRouteInformation { get; set; }

        public string RouteRequirement { get; set; }
    }
}