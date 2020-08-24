using System;

namespace DFC.Api.JobProfiles.IntegrationTests.Model.Support
{
    public class ExpectedResponse
    {
        public ExpectedResponse(string expectedJobProfileDetailsResponse)
        {
            ExpectedJobProfileDetails = expectedJobProfileDetailsResponse;
        }

        private string ExpectedJobProfileDetails { get; set; }

        public string JobProfileDetails(Uri baseUrl, string canonicalName)
        {
            return this.ExpectedJobProfileDetails.Replace("{jobProfileDetailsUrl}", baseUrl == null ? string.Empty : baseUrl.ToString(), StringComparison.InvariantCulture).Replace("{CanonicalName}", canonicalName, StringComparison.InvariantCulture);
        }
    }
}
