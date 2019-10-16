using DFC.App.JobProfiles.HowToBecome.Data.Enums;
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
        public DateTime LastReviewed { get; set; }

        public string Title { get; set; }

        public TitlePrefix TitlePrefix { get; set; }

        public HtmlString EntryRouteSummary { get; set; }

        public EntryRoutes EntryRoutes { get; set; }

        [Display(Name = "More Information")]
        public MoreInformation MoreInformation { get; set; }

        public IEnumerable<GenericListContent> Registrations { get; set; }

        public bool HasRegistrations => Registrations != null && Registrations.Any();

        public bool HasProfessionalAndIndustryBodies => !string.IsNullOrEmpty(MoreInformation?.ProfessionalAndIndustryBodies?.Value);

        public bool HasCareerTips => !string.IsNullOrEmpty(MoreInformation?.CareerTips?.Value);

        public bool HasFurtherInformation => !string.IsNullOrEmpty(MoreInformation?.FurtherInformation?.Value);

        public string GetDynamicTitle()
        {
            switch (TitlePrefix)
            {
                case TitlePrefix.None:
                    return $"{Title}";

                case TitlePrefix.A:
                    return $"a {Title}";

                case TitlePrefix.An:
                    return $"an {Title}";

                default:
                    return GetDefaultDynamicTitle(Title);
            }
        }

        private static string GetDefaultDynamicTitle(string title) => IsStartsWithVowel(title) ? $"an {title}" : $"a {title}";

        private static bool IsStartsWithVowel(string title) => new[] { 'a', 'e', 'i', 'o', 'u' }.Contains(title.ToLowerInvariant().First());
    }
}