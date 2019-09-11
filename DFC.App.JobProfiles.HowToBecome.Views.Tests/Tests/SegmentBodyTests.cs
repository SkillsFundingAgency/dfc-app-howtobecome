using DFC.App.JobProfiles.HowToBecome.ViewModels;
using DFC.App.JobProfiles.HowToBecome.Views.Tests.ViewRenderer;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using Xunit;

namespace DFC.App.JobProfiles.HowToBecome.Views.Tests.Tests
{
    public class SegmentBodyTests : TestsBase
    {
        [Fact]
        public void ContainsContentFromModel()
        {
            const string expectedHeading = "<h2 class=\"heading-large job-profile-heading\">How to become a Nurse</h2>";
            const string expectedSubHeading = "<h1>Nurse Job data</h1>";

            var model = new DocumentViewModel
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = "nurse",
                Markup = new HtmlString("<h1>Nurse Job data</h1>"),
                Title = "Nurse",
                Data = new DocumentDataViewModel { Updated = DateTime.UtcNow },
            };

            var viewBag = new Dictionary<string, object>();
            var viewRenderer = new RazorEngineRenderer(viewRootPath);

            var viewRenderResponse = viewRenderer.Render(@"Body", model, viewBag);

            Assert.Contains(expectedHeading, viewRenderResponse, StringComparison.OrdinalIgnoreCase);
            Assert.Contains(expectedSubHeading, viewRenderResponse, StringComparison.OrdinalIgnoreCase);
        }
    }
}