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
        [Display(Name = "Last Updated")]
        public DateTime LastReviewed { get; set; }

        public string Title { get; set; }

        public TitlePrefix TitlePrefix { get; set; }

        public HtmlString EntryRouteSummary { get; set; }

        public EntryRoutes EntryRoutes { get; set; }

        [Display(Name = "More Information")]
        public MoreInformation MoreInformation { get; set; }

        public IEnumerable<Registration> Registrations { get; set; }

        public bool HasRegistrations => Registrations != null && Registrations.Any();

        public bool HasProfessionalAndIndustryBodies => !string.IsNullOrWhiteSpace(MoreInformation?.ProfessionalAndIndustryBodies?.Value);

        public bool HasCareerTips => !string.IsNullOrWhiteSpace(MoreInformation?.CareerTips?.Value);

        public bool HasFurtherInformation => !string.IsNullOrWhiteSpace(MoreInformation?.FurtherInformation?.Value);

        public string GetDynamicTitle()
        {
            switch (TitlePrefix)
            {
                case TitlePrefix.AsDefined:
                case TitlePrefix.NoPrefix:
                    return $"{Title}";

                case TitlePrefix.NoTitle:
                    return string.Empty;

                case TitlePrefix.PrefixWithA:
                    return $"a {Title}";

                case TitlePrefix.PrefixWithAn:
                    return $"an {Title}";

                default:
                    return GetDefaultDynamicTitle(Title);
            }
        }

        private static string GetDefaultDynamicTitle(string title) => IsStartsWithVowel(title) ? $"an {title}" : $"a {title}";

        private static bool IsStartsWithVowel(string title) => new[] { 'a', 'e', 'i', 'o', 'u' }.Contains(title.ToLowerInvariant().First());
    }
}