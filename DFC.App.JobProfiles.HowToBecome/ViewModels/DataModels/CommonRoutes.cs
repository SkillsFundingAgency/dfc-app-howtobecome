using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;

namespace DFC.App.JobProfiles.HowToBecome.ViewModels.DataModels
{
    public class CommonRoutes
    {
        public RouteName RouteName { get; set; }

        public HtmlString Subject { get; set; }

        public HtmlString FurtherInformation { get; set; }

        public string EntryRequirementPreface { get; set; }

        public IEnumerable<string> EntryRequirements { get; set; }

        public IEnumerable<AdditionalInformation> AdditionalInformation { get; set; }
    }
}