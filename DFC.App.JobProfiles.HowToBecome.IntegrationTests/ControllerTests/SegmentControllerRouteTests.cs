using DFC.App.JobProfiles.HowToBecome.Data.Models;
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
    public class SegmentControllerRouteTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<DataSeeding>
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
            new object[] { $"{SegmentUrl}/{DataSeeding.Job1CanonicalName}/contents", MediaTypeNames.Application.Json },
            new object[] { $"{SegmentUrl}/{DataSeeding.Job1CanonicalName}/contents", MediaTypeNames.Text.Html },
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
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Title = $"{nameof(PostSegmentEndpointsReturnCreated)} created title",
                Data = new HowToBecomeSegmentDataModel
                {
                    Updated = DateTime.UtcNow,
                    Markup = "<h1>Dummy markup value</h1>",
                },
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
                Created = DataSeeding.MainJobDatetime,
                Updated = DateTime.UtcNow,
                Title = $"{nameof(PostSegmentEndpointsForDefaultArticleRefreshAllReturnOk)} created title",
                Data = new HowToBecomeSegmentDataModel
                {
                    Updated = DateTime.UtcNow,
                    Markup = "<h1>Dummy markup value</h1>",
                },
            };

            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.PostAsync(SegmentUrl, howToBecomeSegmentModel, new JsonMediaTypeFormatter()).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PutSegmentEndpointsReturnOk()
        {
            // Arrange
            var howToBecomeSegmentModel = new HowToBecomeSegmentModel
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = Guid.NewGuid().ToString(),
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Title = $"{nameof(PutSegmentEndpointsReturnOk)} created title",
                Data = new HowToBecomeSegmentDataModel
                {
                    Updated = DateTime.UtcNow,
                    Markup = "<h1>Dummy markup value</h1>",
                },
            };
            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            await client.PostAsync(SegmentUrl, howToBecomeSegmentModel, new JsonMediaTypeFormatter()).ConfigureAwait(false);

            // Act
            var response = await client.PutAsync(SegmentUrl, howToBecomeSegmentModel, new JsonMediaTypeFormatter()).ConfigureAwait(false);

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
                Data = new HowToBecomeSegmentDataModel
                {
                    Updated = DateTime.UtcNow,
                    Markup = "<h1>Dummy markup value</h1>",
                },
            };

            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            await client.PostAsync(SegmentUrl, howToBecomeSegmentModel, new JsonMediaTypeFormatter()).ConfigureAwait(false);

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
    }
}