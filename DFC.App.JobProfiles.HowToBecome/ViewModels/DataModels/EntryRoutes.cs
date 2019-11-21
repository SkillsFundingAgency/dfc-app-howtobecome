using Microsoft.AspNetCore.Html;
using System.Collections.Generic;

namespace DFC.App.JobProfiles.HowToBecome.ViewModels.DataModels
{
    public class EntryRoutes
    {
        public IEnumerable<CommonRoutes> CommonRoutes { get; set; }

        public HtmlString Work { get; set; }

        public HtmlString Volunteering { get; set; }

        public HtmlString DirectApplication { get; set; }

        public HtmlString OtherRoutes { get; set; }
    }
}