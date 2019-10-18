using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobProfiles.HowToBecome.IntegrationTests.ControllerTests
{
    [Trait("Integration Tests", "Segment Controller Tests")]
    public class SegmentControllerRouteTests : IClassFixture<CustomWebApplicationFactory<Startup>>,
        IClassFixture<DataSeeding>
    {
        private const string SegmentUrl = "/segment";

        private readonly CustomWebApplicationFactory<Startup> factory;

        public SegmentControllerRouteTests(CustomWebApplicationFactory<Startup> factory, DataSeeding seeding)
        {
            this.factory = factory;
            seeding?.SeedDefaultArticle(factory).GetAwaiter().GetResult();
        }

        public static IEnumerable<object[]> SegmentDocumentRouteData => new List<object[]>
        {
            new object[] { SegmentUrl },
            new object[] { $"{SegmentUrl}/{DataSeeding.Job1CanonicalName}" },
        };

        public static IEnumerable<object[]> MissingSegmentContentRouteData => new List<object[]>
        {
            new object[] { $"{SegmentUrl}/invalid-segment-name" },
        };

        public static IEnumerable<object[]> SegmentBodyRouteData => new List<object[]>
        {
            new object[] { $"{SegmentUrl}/{DataSeeding.MainArticleGuid}/contents", MediaTypeNames.Application.Json },
            new object[] { $"{SegmentUrl}/{DataSeeding.MainArticleGuid}/contents", MediaTypeNames.Text.Html },
        };

        [Theory]
        [MemberData(nameof(SegmentDocumentRouteData))]
        public async Task GetSegmentHtmlContentEndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var uri = new Uri(url, UriKind.Relative);
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.GetAsync(uri).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal($"{MediaTypeNames.Text.Html}; charset={Encoding.UTF8.WebName}", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [MemberData(nameof(MissingSegmentContentRouteData))]
        public async Task GetSegmentHtmlContentEndpointsReturnNoContent(string url)
        {
            // Arrange
            var uri = new Uri(url, UriKind.Relative);
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.GetAsync(uri).ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [MemberData(nameof(SegmentBodyRouteData))]
        public async Task GetSegmentBodyEndpointReturnsSuccessAndCorrectContentType(string url, string mediaType)
        {
            // Arrange
            var uri = new Uri(url, UriKind.Relative);
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(mediaType));

            // Act
            var response = await client.GetAsync(uri).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal($"{mediaType}; charset={Encoding.UTF8.WebName}", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task PostSegmentEndpointsReturnCreated()
        {
            // Arrange
            var howToBecomeSegmentModel = new HowToBecomeSegmentModel
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = Guid.NewGuid().ToString(),
                SocLevelTwo = "12PostSoc",
                Data = GetDefaultHowToBecomeSegmentDataModel(nameof(PostSegmentEndpointsReturnCreated)),
            };

            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.PostAsync(SegmentUrl, howToBecomeSegmentModel, new JsonMediaTypeFormatter()).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task PostSegmentEndpointsForDefaultArticleRefreshAllReturnOk()
        {
            // Arrange
            var howToBecomeSegmentModel = new HowToBecomeSegmentModel
            {
                DocumentId = DataSeeding.MainArticleGuid,
                CanonicalName = DataSeeding.Job1CanonicalName,
                SocLevelTwo = DataSeeding.MainJobSocLevelTwo,
                Data = GetDefaultHowToBecomeSegmentDataModel(nameof(PostSegmentEndpointsForDefaultArticleRefreshAllReturnOk)),
            };

            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.PostAsync(SegmentUrl, howToBecomeSegmentModel, new JsonMediaTypeFormatter()).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task PutSegmentEndpointsReturnOk()
        {
            // Arrange
            var howToBecomeSegmentModel = new HowToBecomeSegmentModel
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = Guid.NewGuid().ToString(),
                SocLevelTwo = "11PutSoc",
                Data = GetDefaultHowToBecomeSegmentDataModel(nameof(PutSegmentEndpointsReturnOk)),
            };
            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            await client.PostAsync(SegmentUrl, howToBecomeSegmentModel, new JsonMediaTypeFormatter())
                .ConfigureAwait(false);

            // Act
            howToBecomeSegmentModel.SequenceNumber++;
            var response = await client.PutAsync(SegmentUrl, howToBecomeSegmentModel, new JsonMediaTypeFormatter())
                .ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteSegmentEndpointsReturnSuccessWhenFound()
        {
            // Arrange
            var documentId = Guid.NewGuid();

            var deleteUri = new Uri($"{SegmentUrl}/{documentId}", UriKind.Relative);

            var howToBecomeSegmentModel = new HowToBecomeSegmentModel
            {
                DocumentId = documentId,
                CanonicalName = documentId.ToString().ToLowerInvariant(),
                SocLevelTwo = "12345",
                Data = new HowToBecomeSegmentDataModel { LastReviewed = DateTime.UtcNow },
            };

            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            await client.PostAsync(SegmentUrl, howToBecomeSegmentModel, new JsonMediaTypeFormatter())
                .ConfigureAwait(false);

            // Act
            var response = await client.DeleteAsync(deleteUri).ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteSegmentEndpointsReturnNotFound()
        {
            // Arrange
            var deleteUri = new Uri($"{SegmentUrl}/{Guid.NewGuid()}", UriKind.Relative);
            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.DeleteAsync(deleteUri).ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        private HowToBecomeSegmentDataModel GetDefaultHowToBecomeSegmentDataModel(string title)
        {
            return new HowToBecomeSegmentDataModel
            {
                LastReviewed = DateTime.UtcNow,
                Title = $"{title} created title",
                TitlePrefix = TitlePrefix.AsDefined,
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
                            EntryRequirements = new List<EntryRequirement>
                            {
                                new EntryRequirement { Id = Guid.NewGuid(), Description = "1 or 2 A levels for a foundation degree or higher national diploma", Rank = 1, Title = "Title 1" },
                                new EntryRequirement { Id = Guid.NewGuid(), Description = "2 to 3 A levels for a degree", Rank = 2, Title = "Title 2" },
                            },
                            AdditionalInformation = new List<AdditionalInformation>
                            {
                                new AdditionalInformation { Link = "https://something", Text = "Equivalent entry requirements" },
                                new AdditionalInformation { Link = "https://something", Text = "Equivalent entry requirements" },
                                new AdditionalInformation { Link = "https://something", Text = "Equivalent entry requirements" },
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
                Registrations = new List<Registration>
                {
                    new Registration { Id = Guid.NewGuid(), Title = "RegistrationTitle1", Description = "Registration 1", Rank = 1 },
                    new Registration { Id = Guid.NewGuid(), Title = "RegistrationTitle2", Description = "Registration 1", Rank = 2 },
                },
            };
        }
    }
}