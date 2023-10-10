using DFC.App.JobProfiles.HowToBecome.Data.Models.DataModels;
using System.Collections.Generic;

namespace DFC.App.JobProfiles.HowToBecome.ApiModels
{
    public class HowToBecomeApiModel
    {
        public List<string> EntryRouteSummary { get; set; }

        public EntryRoutesApiModel EntryRoutes { get; set; }

        public MoreInformationApiModel MoreInformation { get; set; }

        /// <summary>
        /// Gets or sets the social proof real story when one has been selected for the job profile.
        /// </summary>
        /// <value>
        /// A <see cref="DataModels.RealStory"/> when present; otherwise, a value of <c>null</c>.
        /// </value>
        public RealStory RealStory { get; set; }
    }
}