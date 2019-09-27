using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using DFC.App.JobProfiles.HowToBecome.ViewModels;
using DFC.App.JobProfiles.HowToBecome.ViewModels.DataModels;
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
            // Arrange
            const string expectedHeading = "<h2 class=\"heading-large job-profile-heading\">How to become a Web Developer</h2>";

            var model = new DocumentViewModel
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = "web-developer",
                Data = GetDefaultHowToBecomeSegmentDataModel(),
            };

            var viewBag = new Dictionary<string, object>();
            var viewRenderer = new RazorEngineRenderer(viewRootPath);

            // Act
            var viewRenderResponse = viewRenderer.Render(@"Body", model, viewBag);

            // Assert
            Assert.Contains(expectedHeading, viewRenderResponse, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void RouteEntryRendersEntryRequirementsAndAdditionalInformationSectionsWhenInModel()
        {
            // Arrange
            const string expectedEntryRequirementsHeading = "<h4>Entry requirements</h4>";
            const string expectedMoreInformationHeading = "<h4>More Information</h4>";

            var model = new CommonRoutes
            {
                RouteName = RouteName.University,
                Subject = new HtmlString("<p>Subject</p>"),
                EntryRequirementPreface = "Entry Requirement Preface",
                EntryRequirements = new List<EntryRequirements> { new EntryRequirements { Id = "1", Description = "Some entry requirement" } },
                AdditionalInformation = new List<AdditionalInformation>{
                    new AdditionalInformation
                    {
                        Link = "http://Something",
                        Text = "Some Additional Information",
                    },
                },
            };

            var viewBag = new Dictionary<string, object>();
            var viewRenderer = new RazorEngineRenderer(viewRootPath);

            // Act
            var viewRenderResponse = viewRenderer.Render(@"RouteEntry", model, viewBag);

            // Assert
            Assert.Contains(expectedEntryRequirementsHeading, viewRenderResponse, StringComparison.OrdinalIgnoreCase);
            Assert.Contains(expectedMoreInformationHeading, viewRenderResponse, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void RouteEntryDoesNotRenderEntryRequirementsWhenNoneInModel()
        {
            // Arrange
            const string expectedAbsentHeading = "<h4>Entry requirements</h4>";

            var model = new CommonRoutes
            {
                RouteName = RouteName.University,
                EntryRequirementPreface = string.Empty,
                EntryRequirements = new List<EntryRequirements>(),
                Subject = new HtmlString("<p>Subject</p>"),
            };

            var viewBag = new Dictionary<string, object>();
            var viewRenderer = new RazorEngineRenderer(viewRootPath);

            // Act
            var viewRenderResponse = viewRenderer.Render(@"RouteEntry", model, viewBag);

            // Assert
            Assert.DoesNotContain(expectedAbsentHeading, viewRenderResponse, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void RouteEntryDoesNotRenderAdditionalInformationWhenNoneInModel()
        {
            // Arrange
            const string expectedAbsentHeading = "<h4>More Information</h4>";

            var model = new CommonRoutes
            {
                RouteName = RouteName.University,
                Subject = new HtmlString("<p>Subject</p>"),
                AdditionalInformation = new List<AdditionalInformation>(),
            };

            var viewBag = new Dictionary<string, object>();
            var viewRenderer = new RazorEngineRenderer(viewRootPath);

            // Act
            var viewRenderResponse = viewRenderer.Render(@"RouteEntry", model, viewBag);

            // Assert
            Assert.DoesNotContain(expectedAbsentHeading, viewRenderResponse, StringComparison.OrdinalIgnoreCase);
        }

        public static IEnumerable<object[]> FurtherEntryRoutesSet => new List<object[]>
        {
            new object[] { true, true, true, true },
            new object[] { false, true, true, true },
            new object[] { true, false, true, true },
            new object[] { true, true, false, true },
            new object[] { true, true, true, false },
        };

        [Theory]
        [MemberData(nameof(FurtherEntryRoutesSet))]
        public void FurtherRoutesRendersAppropriateSections(bool isWorkSet, bool isDirectAppSet, bool isOtherRoutesSet, bool isVolunteerSet)
        {
            // Arrange
            const string expectedWorkHeading = "<section class=\"job-profile-subsection\" id=\"work\">";
            const string expectedDirectApplicationHeading = "<section class=\"job-profile-subsection\" id=\"directapplication\">";
            const string expectedOtherRoutesHeading = "<section class=\"job-profile-subsection\" id=\"otherroutes\">";
            const string expectedVolunteeringHeading = "<section class=\"job-profile-subsection\" id=\"volunteering\">";

            var model = new EntryRoutes
            {
                Work = isWorkSet ? new HtmlString("Work test value") : null,
                DirectApplication = isDirectAppSet ? new HtmlString("DirectApplication test value") : null,
                OtherRoutes = isOtherRoutesSet ? new HtmlString("OtherRoutes test value") : null,
                Volunteering = isVolunteerSet ? new HtmlString("Volunteering test value") : null,
            };

            var viewBag = new Dictionary<string, object>();
            var viewRenderer = new RazorEngineRenderer(viewRootPath);

            // Act
            var viewRenderResponse = viewRenderer.Render(@"FurtherRoutes", model, viewBag);

            // Assert
            Assert.True(isWorkSet ? viewRenderResponse.Contains(expectedWorkHeading, StringComparison.OrdinalIgnoreCase) : !viewRenderResponse.Contains(expectedWorkHeading, StringComparison.OrdinalIgnoreCase));
            Assert.True(isDirectAppSet ? viewRenderResponse.Contains(expectedDirectApplicationHeading, StringComparison.OrdinalIgnoreCase) : !viewRenderResponse.Contains(expectedDirectApplicationHeading, StringComparison.OrdinalIgnoreCase));
            Assert.True(isOtherRoutesSet ? viewRenderResponse.Contains(expectedOtherRoutesHeading, StringComparison.OrdinalIgnoreCase) : !viewRenderResponse.Contains(expectedOtherRoutesHeading, StringComparison.OrdinalIgnoreCase));
            Assert.True(isVolunteerSet ? viewRenderResponse.Contains(expectedVolunteeringHeading, StringComparison.OrdinalIgnoreCase) : !viewRenderResponse.Contains(expectedVolunteeringHeading, StringComparison.OrdinalIgnoreCase));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MoreInformationRendersRegistrationSectionWhenPresentInModel(bool hasRegistrations)
        {
            const string expectedHeader = "<h4>Registrations</h4>";
            var model = GetDefaultHowToBecomeSegmentDataModel(hasRegistrations);

            var viewBag = new Dictionary<string, object>();
            var viewRenderer = new RazorEngineRenderer(viewRootPath);

            // Act
            var viewRenderResponse = viewRenderer.Render(@"MoreInformation", model, viewBag);
            Assert.True(hasRegistrations ? viewRenderResponse.Contains(expectedHeader, StringComparison.OrdinalIgnoreCase) : !viewRenderResponse.Contains(expectedHeader, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<object[]> MoreInformationPropertiesSet => new List<object[]>
        {
            new object[] { true, true, true},
            new object[] { false, true, true},
            new object[] { true, false, true},
            new object[] { true, true, false},
        };

        [Theory]
        [MemberData(nameof(MoreInformationPropertiesSet))]
        public void MoreInformationRendersPropertiesWhenSetOnModel(bool isProfessionalBodiesSet, bool isCareerTipsSet, bool isFurtherInfoSet)
        {
            const string professionalBodiesText = "<p>Professional and Industry Bodies</p>";
            const string careerTipsText = "<p>Career Tips</p>";
            const string furtherInformationText = "<p>Further Information</p>";

            var model = new DocumentDataViewModel
            {
                Registrations = new List<string> { "Registration 1", "Registration 2" },
                MoreInformation = new MoreInformation
                {
                    ProfessionalAndIndustryBodies = isProfessionalBodiesSet ? new HtmlString(professionalBodiesText) : null,
                    CareerTips = isCareerTipsSet ? new HtmlString(careerTipsText) : null,
                    FurtherInformation = isFurtherInfoSet ? new HtmlString(furtherInformationText) : null,
                },
            };

            var viewBag = new Dictionary<string, object>();
            var viewRenderer = new RazorEngineRenderer(viewRootPath);

            // Act
            var viewRenderResponse = viewRenderer.Render(@"MoreInformation", model, viewBag);
            Assert.True(isProfessionalBodiesSet ? viewRenderResponse.Contains(professionalBodiesText, StringComparison.OrdinalIgnoreCase) : !viewRenderResponse.Contains(professionalBodiesText, StringComparison.OrdinalIgnoreCase));
            Assert.True(isCareerTipsSet ? viewRenderResponse.Contains(careerTipsText, StringComparison.OrdinalIgnoreCase) : !viewRenderResponse.Contains(careerTipsText, StringComparison.OrdinalIgnoreCase));
            Assert.True(isFurtherInfoSet ? viewRenderResponse.Contains(furtherInformationText, StringComparison.OrdinalIgnoreCase) : !viewRenderResponse.Contains(furtherInformationText, StringComparison.OrdinalIgnoreCase));
        }

        private DocumentDataViewModel GetDefaultHowToBecomeSegmentDataModel(bool hasRegistrations = true)
        {
            return new DocumentDataViewModel
            {
                LastReviewed = DateTime.UtcNow,
                Title = "Web Developer",
                TitlePrefix = TitlePrefix.Default,
                EntryRouteSummary = new HtmlString("<p>You can get into this job through:</p><ul><li>a university course </li><li> a college course </li><li> an apprenticeship </li><li> working towards this role </li></ul>"),
                EntryRoutes = new EntryRoutes
                {
                    CommonRoutes = new List<CommonRoutes>
                    {
                        new CommonRoutes
                        {
                            RouteName = RouteName.University,
                            Subject = new HtmlString("<p>You could do a foundation degree, higher national diploma or  degree in:</p><ul><li>web design and development</li><li>computer science</li><li>digital media development</li><li>software engineering</li></ul>"),
                            FurtherInformation = new HtmlString("<p>Further information</p>"),
                            EntryRequirementPreface = "You will usually need:",
                            EntryRequirements = new List<EntryRequirements>
                            {
                                new EntryRequirements{ Id = "1", Description = "1 or 2 A levels for a foundation degree or higher national diploma", Rank = 1},
                                new EntryRequirements{ Id = "2", Description = "2 to 3 A levels for a degree", Rank = 2},
                            },
                            AdditionalInformation = new List<AdditionalInformation>
                            {
                                new AdditionalInformation {Link = "https://something", Text = "Equivalent entry requirements"},
                                new AdditionalInformation {Link = "https://something", Text = "Equivalent entry requirements"},
                                new AdditionalInformation {Link = "https://something", Text = "Equivalent entry requirements"},
                            },
                        },
                    },
                    Work = new HtmlString("<p>You may be able to start as a junior developer and improve your skills and knowledge by completing further training and qualifications while you work.</p>"),
                    Volunteering = new HtmlString("<p>Volunteering information</p>"),
                    DirectApplication = new HtmlString("<p>Direct application information</p>"),
                    OtherRoutes = new HtmlString("<p>Other routes information</p>"),
                },
                MoreInformation = new MoreInformation
                {
                    FurtherInformation = new HtmlString("<h4>Further information </h4><p>You can get more advice about working in computing from <a href='https://www.tpdegrees.com/careers/'>Tech Future Careers</a> and<a href = 'https://www.bcs.org/category/5672'> The Chartered Institute for IT.</a></p> "),
                    ProfessionalAndIndustryBodies = new HtmlString("<p>Professional and Industry bodies here</p>"),
                    CareerTips = new HtmlString("<h4>Career tips</h4><p>Make sure that you're up to date with the latest industry trends and web development standards.</p>"),
                },
                Registrations = hasRegistrations ? new List<string> { "Registration 1", "Registration 2" } : null,
            };
        }
    }
}