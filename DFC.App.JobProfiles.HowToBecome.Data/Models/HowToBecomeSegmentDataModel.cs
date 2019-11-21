using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using DFC.App.JobProfiles.HowToBecome.Data.Models.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DFC.App.JobProfiles.HowToBecome.Data.Models
{
    public class HowToBecomeSegmentDataModel
    {
        public const string SegmentName = "HowToBecome";

        public DateTime LastReviewed { get; set; }

        public string Title { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TitlePrefix TitlePrefix { get; set; }

        public string EntryRouteSummary { get; set; }

        public EntryRoutes EntryRoutes { get; set; }

        public MoreInformation MoreInformation { get; set; }

        public IList<Registration> Registrations { get; set; }
    }
}