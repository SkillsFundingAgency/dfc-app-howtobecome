using System.Collections.Generic;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model
{
    public class EntryRequirement
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
    }

    public class MoreInformationLink
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Text { get; set; }
    }

    public class HowToBecomeRouteEntry
    {
        public int RouteName { get; set; }
        public List<EntryRequirement> EntryRequirements { get; set; }
        public List<MoreInformationLink> MoreInformationLinks { get; set; }
        public string RouteSubjects { get; set; }
        public string FurtherRouteInformation { get; set; }
        public string RouteRequirement { get; set; }
    }
}
