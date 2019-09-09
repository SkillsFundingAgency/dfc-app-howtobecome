using DFC.App.JobProfiles.HowToBecome.Data.Models;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace DFC.App.JobProfiles.HowToBecome.IntegrationTests
{
    public static class DataSeeding
    {
        private static readonly Guid MainArticleGuid = Guid.Parse("e2156143-e951-4570-a7a0-16f999f68661");

        public static void SeedDefaultArticle(CustomWebApplicationFactory<Startup> factory, string article)
        {
            const string url = "/segment";

            var model = new HowToBecomeSegmentModel
            {
                DocumentId = MainArticleGuid,
                CanonicalName = article,
                Title = article.ToUpperInvariant(),
                Data = new HowToBecomeSegmentDataModel
                {
                    LastReviewed = DateTime.UtcNow,
                },
            };

            var client = factory?.CreateClient();

            client?.DefaultRequestHeaders.Accept.Clear();

            client.PostAsync(url, model, new JsonMediaTypeFormatter());
        }
    }
}