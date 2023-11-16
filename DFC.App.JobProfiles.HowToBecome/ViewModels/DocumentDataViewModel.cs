using DFC.App.JobProfiles.HowToBecome.ViewModels.DataModels;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DFC.App.JobProfiles.HowToBecome.ViewModels
{
    public class DocumentDataViewModel
    {
        [Display(Name = "Last Updated")]
        public DateTime LastReviewed { get; set; }

        public string Title { get; set; }

        public HtmlString EntryRouteSummary { get; set; }

        public EntryRoutes EntryRoutes { get; set; }

        [Display(Name = "More Information")]
        public MoreInformation MoreInformation { get; set; }

        public IEnumerable<Registration> Registrations { get; set; }

        public bool HasRegistrations => Registrations != null && Registrations.Any();

        public bool HasProfessionalAndIndustryBodies => !string.IsNullOrWhiteSpace(MoreInformation?.ProfessionalAndIndustryBodies?.Value);

        public bool HasCareerTips => !string.IsNullOrWhiteSpace(MoreInformation?.CareerTips?.Value);

        public bool HasFurtherInformation => !string.IsNullOrWhiteSpace(MoreInformation?.FurtherInformation?.Value);

        public RealStory RealStory { get; set; }
    }
}