using DFC.Api.JobProfiles.IntegrationTests.Support.API;
using DFC.App.JobProfiles.HowToBecome.FunctionalTests.Model.APIResponse;
using DFC.App.JobProfiles.HowToBecome.FunctionalTests.Model.ContentType;
using DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support;
using DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.API;
using DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.API.RestFactory;
using DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.AzureServiceBus.ServiceBusFactory;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.FunctionalTests.Test
{
    public class MoreInformationLink : SetUpAndTearDown
    {
        private JobProfileApi howToBecomeAPI;

        private RouteEntry universityRouteEntry;

        private RouteEntry collegeRouteEntry;

        private RouteEntry apprenticeshipRouteEntry;

        [SetUp]
        public void SetUp()
        {
            var apiSettings = new APISettings { Endpoint = new Uri(this.AppSettings.APIConfig.EndpointBaseUrl) };
            this.howToBecomeAPI = new JobProfileApi(new RestClientFactory(), new RestRequestFactory(), this.AppSettings, apiSettings);
            this.universityRouteEntry = this.JobProfile.HowToBecomeData.RouteEntries.Where(re => re.RouteName.Equals((int)RouteEntryType.University)).FirstOrDefault();
            this.collegeRouteEntry = this.JobProfile.HowToBecomeData.RouteEntries.Where(re => re.RouteName.Equals((int)RouteEntryType.College)).FirstOrDefault();
            this.apprenticeshipRouteEntry = this.JobProfile.HowToBecomeData.RouteEntries.Where(re => re.RouteName.Equals((int)RouteEntryType.Apprenticeship)).FirstOrDefault();
        }

        [Test]
        public async Task JobProfileHowToBecomeUniversityLink()
        {
            LinksContentType linksContentType = new LinksContentType()
            {
                Id = this.universityRouteEntry.MoreInformationLinks[0].Id,
                Text = "This is updated link text",
                Title = "This is an updated link title",
                Url = new Uri($"https://{this.CommonAction.RandomString(10)}.com/"),
                JobProfileId = this.JobProfile.JobProfileId,
                JobProfileTitle = this.JobProfile.Title,
            };

            var messageBody = this.CommonAction.ConvertObjectToByteArray(linksContentType);
            var message = new MessageFactory().Create(this.JobProfile.JobProfileId, messageBody, "Published", "UniversityLink");
            await this.ServiceBus.SendMessage(message).ConfigureAwait(false);
            await Task.Delay(5000).ConfigureAwait(true);
            var response = await this.howToBecomeAPI.GetById<HowToBecomeAPIResponse>(this.JobProfile.JobProfileId).ConfigureAwait(true);
            Assert.AreEqual($"[{linksContentType.Text} | {linksContentType.Url.ToString().ToLower(CultureInfo.CurrentCulture)}]", response.Data.EntryRoutes.University.AdditionalInformation[0]);
        }

        [Test]
        public async Task JobProfileHowToBecomeCollegeLink()
        {
            LinksContentType linksContentType = new LinksContentType()
            {
                Id = this.collegeRouteEntry.MoreInformationLinks[0].Id,
                Text = "This is updated link text",
                Title = "This is an updated link title",
                Url = new Uri($"https://{this.CommonAction.RandomString(10)}.com/"),
                JobProfileId = this.JobProfile.JobProfileId,
                JobProfileTitle = this.JobProfile.Title,
            };

            var messageBody = this.CommonAction.ConvertObjectToByteArray(linksContentType);
            var message = new MessageFactory().Create(this.JobProfile.JobProfileId, messageBody, "Published", "CollegeLink");
            await this.ServiceBus.SendMessage(message).ConfigureAwait(false);
            await Task.Delay(5000).ConfigureAwait(true);
            var response = await this.howToBecomeAPI.GetById<HowToBecomeAPIResponse>(this.JobProfile.JobProfileId).ConfigureAwait(true);
            Assert.AreEqual($"[{linksContentType.Text} | {linksContentType.Url.ToString().ToLower(CultureInfo.CurrentCulture)}]", response.Data.EntryRoutes.College.AdditionalInformation[0]);
        }

        [Test]
        public async Task JobProfileHowToBecomeApprenticeshipLink()
        {
            LinksContentType linksContentType = new LinksContentType()
            {
                Id = this.apprenticeshipRouteEntry.MoreInformationLinks[0].Id,
                Text = "This is updated link text",
                Title = "This is an updated link title",
                Url = new Uri($"https://{this.CommonAction.RandomString(10)}.com/"),
                JobProfileId = this.JobProfile.JobProfileId,
                JobProfileTitle = this.JobProfile.Title,
            };

            var messageBody = this.CommonAction.ConvertObjectToByteArray(linksContentType);
            var message = new MessageFactory().Create(this.JobProfile.JobProfileId, messageBody, "Published", "ApprenticeshipLink");
            await this.ServiceBus.SendMessage(message).ConfigureAwait(false);
            await Task.Delay(5000).ConfigureAwait(true);
            var response = await this.howToBecomeAPI.GetById<HowToBecomeAPIResponse>(this.JobProfile.JobProfileId).ConfigureAwait(true);
            Assert.AreEqual($"[{linksContentType.Text} | {linksContentType.Url.ToString().ToLower(CultureInfo.CurrentCulture)}]", response.Data.EntryRoutes.Apprenticeship.AdditionalInformation[0]);
        }
    }
}