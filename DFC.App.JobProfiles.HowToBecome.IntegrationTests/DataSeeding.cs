using DFC.App.JobProfiles.HowToBecome.Data.Models;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.IntegrationTests
{
    public class DataSeeding
    {
        internal const string Job1CanonicalName = "nurse";
        internal static readonly Guid MainArticleGuid = Guid.Parse("e2156143-e951-4570-a7a0-16f999f68661");
        internal static readonly DateTime MainJobDatetime = new DateTime(2019, 1, 15, 15, 30, 11);

        private const string Job1Title = "Nurse Title";

        public async Task SeedDefaultArticle(CustomWebApplicationFactory<Startup> factory)
        {
            const string url = "/segment";

            var model = new HowToBecomeSegmentModel
            {
                DocumentId = MainArticleGuid,
                CanonicalName = Job1CanonicalName,
                Title = Job1Title,
                Created = MainJobDatetime,
                Updated = DateTime.UtcNow,
                Data = new HowToBecomeSegmentDataModel
                {
                    Updated = DateTime.UtcNow,
                    Markup = "<h1>Nurse Job data</h1>",
                },
            };

            var client = factory?.CreateClient();

            client?.DefaultRequestHeaders.Accept.Clear();

            await client.PostAsync(url, model, new JsonMediaTypeFormatter()).ConfigureAwait(false);
        }
    }
}