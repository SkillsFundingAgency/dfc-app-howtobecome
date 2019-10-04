using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using DFC.App.JobProfiles.HowToBecome.Data.Models.DataModels;
using System;
using System.Collections.Generic;

namespace DFC.App.JobProfiles.HowToBecome.Data.Models
{
    public class HowToBecomeSegmentDataModel
    {
        public DateTime LastReviewed { get; set; }

        public string Title { get; set; }

        public TitlePrefix TitlePrefix { get; set; }

        public string EntryRouteSummary { get; set; }

        public EntryRoutes EntryRoutes { get; set; }

        public MoreInformation MoreInformation { get; set; }

        public IEnumerable<GenericListContent> Registrations { get; set; }
    }
}