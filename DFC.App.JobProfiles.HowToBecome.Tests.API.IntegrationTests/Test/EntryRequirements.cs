using DFC.Api.JobProfiles.Common.APISupport;
using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Test
{
    public class EntryRequirements : SetUpAndTearDown
    {
        [Test]
        public async Task JobProfileHowToBecomeUniversityEntryRequirements()
        {
            EntryRequirementsClassification entryRequirementsClassification = this.CommonAction.GenerateEntryRequirementsClassificationForJobProfile(RouteEntryType.University, this.JobProfile);
            byte[] messageBody = this.CommonAction.ConvertObjectToByteArray(entryRequirementsClassification);
            Message message = this.CommonAction.CreateServiceBusMessage(this.JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.UniversityEntryRequirements);
            await this.Topic.SendAsync(message).ConfigureAwait(false);
            await Task.Delay(5000).ConfigureAwait(true);
            Response<HowToBecomeAPIResponse> howToBecomeResponse = await this.CommonAction.ExecuteGetRequest<HowToBecomeAPIResponse>(Settings.APIConfig.EndpointBaseUrl.Replace("{id}", this.JobProfile.JobProfileId, StringComparison.InvariantCultureIgnoreCase)).ConfigureAwait(true);
            Assert.AreEqual(entryRequirementsClassification.Title, howToBecomeResponse.Data.entryRoutes.university.entryRequirementPreface);
        }

        [Test]
        public async Task JobProfileHowToBecomeCollegeEntryRequirements()
        {
            EntryRequirementsClassification entryRequirementsClassification = this.CommonAction.GenerateEntryRequirementsClassificationForJobProfile(RouteEntryType.College, this.JobProfile);
            byte[] messageBody = this.CommonAction.ConvertObjectToByteArray(entryRequirementsClassification);
            Message message = this.CommonAction.CreateServiceBusMessage(this.JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.CollegeEntryRequirements);
            await this.Topic.SendAsync(message).ConfigureAwait(true);
            await Task.Delay(5000).ConfigureAwait(true);
            Response<HowToBecomeAPIResponse> howToBecomeResponse = await this.CommonAction.ExecuteGetRequest<HowToBecomeAPIResponse>(Settings.APIConfig.EndpointBaseUrl.Replace("{id}", this.JobProfile.JobProfileId, StringComparison.InvariantCultureIgnoreCase)).ConfigureAwait(true);
            Assert.AreEqual(entryRequirementsClassification.Title, howToBecomeResponse.Data.entryRoutes.college.entryRequirementPreface);
        }

        [Test]
        public async Task JobProfileHowToBecomeApprenticeshipEntryRequirements()
        {
            EntryRequirementsClassification entryRequirementsClassification = this.CommonAction.GenerateEntryRequirementsClassificationForJobProfile(RouteEntryType.Apprenticeship, this.JobProfile);
            byte[] messageBody = this.CommonAction.ConvertObjectToByteArray(entryRequirementsClassification);
            Message message = this.CommonAction.CreateServiceBusMessage(this.JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.ApprenticeshipEntryRequirements);
            await this.Topic.SendAsync(message).ConfigureAwait(true);
            await Task.Delay(5000).ConfigureAwait(true);
            Response<HowToBecomeAPIResponse> howToBecomeResponse = await this.CommonAction.ExecuteGetRequest<HowToBecomeAPIResponse>(Settings.APIConfig.EndpointBaseUrl.Replace("{id}", this.JobProfile.JobProfileId, StringComparison.InvariantCultureIgnoreCase)).ConfigureAwait(true);
            Assert.AreEqual(entryRequirementsClassification.Title, howToBecomeResponse.Data.entryRoutes.apprenticeship.entryRequirementPreface);
        }
    }
}