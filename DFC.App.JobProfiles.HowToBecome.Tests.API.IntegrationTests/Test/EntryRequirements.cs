using DFC.Api.JobProfiles.IntegrationTests.Support.API;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.APIResponse;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.Classification;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.API;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.API.RestFactory;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.AzureServiceBus.ServiceBusFactory;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Test
{
    public class EntryRequirements : SetUpAndTearDown
    {
        private JobProfileApi howToBecomeAPI;

        [SetUp]
        public void SetUp()
        {
            var apiSettings = new APISettings { Endpoint = new Uri(this.AppSettings.APIConfig.EndpointBaseUrl) };
            this.howToBecomeAPI = new JobProfileApi(new RestClientFactory(), new RestRequestFactory(), this.AppSettings, apiSettings);
        }

        [Test]
        public async Task JobProfileHowToBecomeUniversityEntryRequirements()
        {
            var entryRequirementsClassification = new EntryRequirementsClassification()
            {
                Id = this.JobProfile.HowToBecomeData.RouteEntries[(int)RouteEntryType.University].EntryRequirements[0].Id,
                Description = $"This is an updated description for the entry requirement for the university route entry",
                Title = $"This is an updated title for the entry requirement for the university route entry",
                Url = $"https://{this.CommonAction.RandomString(10)}.com/",
                JobProfileId = this.JobProfile.JobProfileId,
                JobProfileTitle = this.JobProfile.Title,
            };

            var messageBody = this.CommonAction.ConvertObjectToByteArray(entryRequirementsClassification);
            var message = new MessageFactory().Create(this.JobProfile.JobProfileId, messageBody, "Published", "UniversityEntryRequirements");
            await this.ServiceBus.SendMessage(message).ConfigureAwait(false);
            await Task.Delay(5000).ConfigureAwait(true);

            var response = await this.howToBecomeAPI.GetById<HowToBecomeAPIResponse>(this.JobProfile.JobProfileId).ConfigureAwait(true);
            Assert.AreEqual(entryRequirementsClassification.Title, response.Data.EntryRoutes.University.EntryRequirementPreface);
        }

        [Test]
        public async Task JobProfileHowToBecomeCollegeEntryRequirements()
        {
            var entryRequirementsClassification = new EntryRequirementsClassification()
            {
                Id = this.JobProfile.HowToBecomeData.RouteEntries[(int)RouteEntryType.College].EntryRequirements[0].Id,
                Description = $"This is an updated description for the entry requirement for the college route entry",
                Title = $"This is an updated title for the entry requirement for the college route entry",
                Url = $"https://{this.CommonAction.RandomString(10)}.com/",
                JobProfileId = this.JobProfile.JobProfileId,
                JobProfileTitle = this.JobProfile.Title,
            };

            var messageBody = this.CommonAction.ConvertObjectToByteArray(entryRequirementsClassification);
            var message = new MessageFactory().Create(this.JobProfile.JobProfileId, messageBody, "Published", "CollegeEntryRequirements");
            await this.ServiceBus.SendMessage(message).ConfigureAwait(false);
            await Task.Delay(5000).ConfigureAwait(true);

            var response = await this.howToBecomeAPI.GetById<HowToBecomeAPIResponse>(this.JobProfile.JobProfileId).ConfigureAwait(true);
            Assert.AreEqual(entryRequirementsClassification.Title, response.Data.EntryRoutes.College.EntryRequirementPreface);
        }

        [Test]
        public async Task JobProfileHowToBecomeApprenticeshipEntryRequirements()
        {
            var entryRequirementsClassification = new EntryRequirementsClassification()
            {
                Id = this.JobProfile.HowToBecomeData.RouteEntries[(int)RouteEntryType.Apprenticeship].EntryRequirements[0].Id,
                Description = $"This is an updated description for the entry requirement for the apprenticeships route entry",
                Title = $"This is an updated title for the entry requirement for the apprenticeships route entry",
                Url = $"https://{this.CommonAction.RandomString(10)}.com/",
                JobProfileId = this.JobProfile.JobProfileId,
                JobProfileTitle = this.JobProfile.Title,
            };

            var messageBody = this.CommonAction.ConvertObjectToByteArray(entryRequirementsClassification);
            var message = new MessageFactory().Create(this.JobProfile.JobProfileId, messageBody, "Published", "ApprenticeshipEntryRequirements");
            await this.ServiceBus.SendMessage(message).ConfigureAwait(false);
            await Task.Delay(5000).ConfigureAwait(true);

            var response = await this.howToBecomeAPI.GetById<HowToBecomeAPIResponse>(this.JobProfile.JobProfileId).ConfigureAwait(true);
            Assert.AreEqual(entryRequirementsClassification.Title, response.Data.EntryRoutes.Apprenticeship.EntryRequirementPreface);
        }
    }
}