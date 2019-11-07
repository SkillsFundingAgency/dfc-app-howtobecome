using System.Collections;
using System.Collections.Generic;

namespace DFC.App.JobProfiles.HowToBecome.ApiModels
{
    public class HowToBecomeApiModel
    {
        public List<string> EntryRouteSummary { get; set; }

        public EntryRoutesApiModel EntryRoutes { get; set; }

        public MoreInformationApiModel MoreInformation { get; set; }
    }
}