using DFC.Api.JobProfiles.IntegrationTests.Support.API;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.APIResponse;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.ContentType;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.API;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.API.RestFactory;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.AzureServiceBus.ServiceBusFactory;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Test
{
    public class Requirement : SetUpAndTearDown
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
        public async Task JobProfileHowToBecomeUniversityRequirement()
        {
            RequirementContentType requirementContentType = new RequirementContentType()
            {
                Id = this.universityRouteEntry.EntryRequirements[0].Id,
                Info = "This is updated requirement info for university",
                Title = "This is an updated requirement title",
                JobProfileId = this.JobProfile.JobProfileId,
                JobProfileTitle = this.JobProfile.Title,
            };

            var messageBody = this.CommonAction.ConvertObjectToByteArray(requirementContentType);
            var message = new MessageFactory().Create(this.JobProfile.JobProfileId, messageBody, "Published", "UniversityRequirement");
            await this.ServiceBus.SendMessage(message).ConfigureAwait(false);
            await Task.Delay(5000).ConfigureAwait(true);
            var response = await this.howToBecomeAPI.GetById<HowToBecomeAPIResponse>(this.JobProfile.JobProfileId).ConfigureAwait(true);
            Assert.AreEqual(requirementContentType.Info, response.Data.EntryRoutes.University.EntryRequirements[0]);
        }

        [Test]
        public async Task JobProfileHowToBecomeCollegeRequirement()
        {
            RequirementContentType requirementContentType = new RequirementContentType()
            {
                Id = this.collegeRouteEntry.EntryRequirements[0].Id,
                Info = "This is updated requirement info for college",
                Title = "This is an updated requirement title",
                JobProfileId = this.JobProfile.JobProfileId,
                JobProfileTitle = this.JobProfile.Title,
            };

            var messageBody = this.CommonAction.ConvertObjectToByteArray(requirementContentType);
            var message = new MessageFactory().Create(this.JobProfile.JobProfileId, messageBody, "Published", "CollegeRequirement");
            await this.ServiceBus.SendMessage(message).ConfigureAwait(false);
            await Task.Delay(5000).ConfigureAwait(true);
            var response = await this.howToBecomeAPI.GetById<HowToBecomeAPIResponse>(this.JobProfile.JobProfileId).ConfigureAwait(true);
            Assert.AreEqual(requirementContentType.Info, response.Data.EntryRoutes.College.EntryRequirements[0]);
        }

        [Test]
        public async Task JobProfileHowToBecomeApprenticeshipRequirement()
        {
            RequirementContentType requirementContentType = new RequirementContentType()
            {
                Id = this.apprenticeshipRouteEntry.EntryRequirements[0].Id,
                Info = "This is updated requirement info for college",
                Title = "This is an updated requirement title",
                JobProfileId = this.JobProfile.JobProfileId,
                JobProfileTitle = this.JobProfile.Title,
            };

            var messageBody = this.CommonAction.ConvertObjectToByteArray(requirementContentType);
            var message = new MessageFactory().Create(this.JobProfile.JobProfileId, messageBody, "Published", "ApprenticeshipRequirement");
            await this.ServiceBus.SendMessage(message).ConfigureAwait(false);
            await Task.Delay(5000).ConfigureAwait(true);
            var response = await this.howToBecomeAPI.GetById<HowToBecomeAPIResponse>(this.JobProfile.JobProfileId).ConfigureAwait(true);
            Assert.AreEqual(requirementContentType.Info, response.Data.EntryRoutes.Apprenticeship.EntryRequirements[0]);
        }
    }
}