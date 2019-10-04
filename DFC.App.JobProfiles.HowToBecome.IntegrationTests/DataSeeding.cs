using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.IntegrationTests
{
    public class DataSeeding
    {
        internal const string Job1CanonicalName = "webdeveloper";
        internal static readonly Guid MainArticleGuid = Guid.Parse("e2156143-e951-4570-a7a0-16f999f68661");
        internal static readonly string MainJobSocLevelTwo = "12345Soc";

        private const string Job1Title = "Web Developer";

        public async Task SeedDefaultArticle(CustomWebApplicationFactory<Startup> factory)
        {
            const string url = "/segment";

            var model = new HowToBecomeSegmentModel
            {
                DocumentId = MainArticleGuid,
                CanonicalName = Job1CanonicalName,
                LastReviewed = DateTime.UtcNow,
                SocLevelTwo = MainJobSocLevelTwo,
                Data = GetDefaultHowToBecomeSegmentDataModel(),
            };

            var client = factory?.CreateClient();

            client?.DefaultRequestHeaders.Accept.Clear();

            await client.PostAsync(url, model, new JsonMediaTypeFormatter()).ConfigureAwait(false);
        }

        private HowToBecomeSegmentDataModel GetDefaultHowToBecomeSegmentDataModel()
        {
            return new HowToBecomeSegmentDataModel
            {
                LastReviewed = DateTime.UtcNow,
                Title = $"{Job1Title} created title",
                TitlePrefix = TitlePrefix.Default,
                EntryRouteSummary = "<p>You can get into this job through:</p><ul><li>a university course </li><li> a college course </li><li> an apprenticeship </li><li> working towards this role </li></ul>",
                EntryRoutes = new EntryRoutes
                {
                    CommonRoutes = new List<CommonRoutes>
                    {
                        new CommonRoutes
                        {
                            RouteName = RouteName.University,
                            Subject = "<p>You could do a foundation degree, higher national diploma or  degree in:</p><ul><li>web design and development</li><li>computer science</li><li>digital media development</li><li>software engineering</li></ul>",
                            FurtherInformation = "<p>Further information</p>",
                            EntryRequirementPreface = "You will usually need:",
                            EntryRequirements = new List<GenericListContent>
                            {
                                new GenericListContent{ Id = "1", Description = "1 or 2 A levels for a foundation degree or higher national diploma", Rank = 1},
                                new GenericListContent{ Id = "2", Description = "2 to 3 A levels for a degree", Rank = 2},
                            },
                            AdditionalInformation = new List<AdditionalInformation>
                            {
                                new AdditionalInformation {Link = "https://something", Text = "Equivalent entry requirements"},
                                new AdditionalInformation {Link = "https://something", Text = "Equivalent entry requirements"},
                                new AdditionalInformation {Link = "https://something", Text = "Equivalent entry requirements"},
                            },
                        },
                    },
                    Work = "<p>You may be able to start as a junior developer and improve your skills and knowledge by completing further training and qualifications while you work.</p>",
                    Volunteering = "<p>Volunteering information</p>",
                    DirectApplication = "<p>Direct application information</p>",
                    OtherRoutes = "<p>Other routes information</p>",
                },
                MoreInformation = new MoreInformation
                {
                    FurtherInformation = "<h4>Further information </h4><p>You can get more advice about working in computing from <a href='https://www.tpdegrees.com/careers/'>Tech Future Careers</a> and<a href = 'https://www.bcs.org/category/5672'> The Chartered Institute for IT.</a></p> ",
                    ProfessionalAndIndustryBodies = "<p>Professional and Industry bodies here</p>",
                    CareerTips = "<h4>Career tips</h4><p>Make sure that you're up to date with the latest industry trends and web development standards.</p>",
                },
                Registrations = new List<GenericListContent> {
                    new GenericListContent{ Id = "1", Description = "Registration 1", Rank = 1},
                    new GenericListContent{ Id = "2", Description = "Registration 1", Rank = 2},
                },
            };
        }
    }
}