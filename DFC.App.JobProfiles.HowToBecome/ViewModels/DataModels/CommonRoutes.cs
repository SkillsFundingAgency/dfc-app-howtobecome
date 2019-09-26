using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using System.Linq;

namespace DFC.App.JobProfiles.HowToBecome.ViewModels.DataModels
{
    public class CommonRoutes
    {
        public RouteName RouteName { get; set; }

        public HtmlString Subject { get; set; }

        public HtmlString FurtherInformation { get; set; }

        public string EntryRequirementPreface { get; set; }

        public IEnumerable<EntryRequirements> EntryRequirements { get; set; }

        public IEnumerable<AdditionalInformation> AdditionalInformation { get; set; }

        public bool HasSubject => !string.IsNullOrEmpty(Subject.Value);

        public bool HasFurtherInformation => !string.IsNullOrEmpty(FurtherInformation.Value);

        public bool HasEntryRequirementPreface => !string.IsNullOrEmpty(EntryRequirementPreface);

        public bool HasEntryRequirements => EntryRequirements != null && EntryRequirements.Any();

        public bool HasAdditionalInformation => AdditionalInformation != null && AdditionalInformation.Any();
    }
}