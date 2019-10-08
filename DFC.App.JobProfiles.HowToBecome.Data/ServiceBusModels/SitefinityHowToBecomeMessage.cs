using System.Collections.Generic;

namespace DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels
{
    public class SitefinityHowToBecomeMessage
    {
        public IEnumerable<SitefinityRouteEntries> RouteEntries { get; set; }

        public SitefinityFurtherMoreInformation FurtherMoreInformation { get; set; }

        public SitefinityFurtherRoutes FurtherRoutes { get; set; }

        public string IntroText { get; set; }

        public List<SitefinityRegistrations> Registrations { get; set; }
    }
}