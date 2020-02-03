using DFC.Api.JobProfiles.Common.APISupport;
using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using NUnit.Framework;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Test
{
    public class EntryRequirements : SetUpAndTearDown
    {
        [Test]
        public async Task JobProfile_HowToBecome_UniversityEntryRequirements()
        {
            EntryRequirementsClassification entryRequirementsClassification = CommonAction.GenerateEntryRequirementsClassificationForJobProfile(RouteEntryType.University, JobProfile);
            byte[] messageBody = CommonAction.ConvertObjectToByteArray(entryRequirementsClassification);
            Message message = CommonAction.CreateServiceBusMessage(JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.UniversityEntryRequirements);
            await Topic.SendAsync(message);
            await Task.Delay(5000);
            Response<HowToBecomeAPIResponse> howToBecomeResponse = await CommonAction.ExecuteGetRequest<HowToBecomeAPIResponse>(Settings.APIConfig.EndpointBaseUrl.JSONContent.Replace("{id}", JobProfile.JobProfileId));
            Assert.AreEqual(entryRequirementsClassification.Title, howToBecomeResponse.Data.entryRoutes.university.entryRequirementPreface);
        }

        [Test]
        public async Task JobProfile_HowToBecome_CollegeEntryRequirements()
        {
            EntryRequirementsClassification entryRequirementsClassification = CommonAction.GenerateEntryRequirementsClassificationForJobProfile(RouteEntryType.College, JobProfile);
            byte[] messageBody = CommonAction.ConvertObjectToByteArray(entryRequirementsClassification);
            Message message = CommonAction.CreateServiceBusMessage(JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.CollegeEntryRequirements);
            await Topic.SendAsync(message);
            await Task.Delay(5000);
            Response<HowToBecomeAPIResponse> howToBecomeResponse = await CommonAction.ExecuteGetRequest<HowToBecomeAPIResponse>(Settings.APIConfig.EndpointBaseUrl.JSONContent.Replace("{id}", JobProfile.JobProfileId));
            Assert.AreEqual(entryRequirementsClassification.Title, howToBecomeResponse.Data.entryRoutes.college.entryRequirementPreface);
        }

        [Test]
        public async Task JobProfile_HowToBecome_ApprenticeshipEntryRequirements()
        {
            EntryRequirementsClassification entryRequirementsClassification = CommonAction.GenerateEntryRequirementsClassificationForJobProfile(RouteEntryType.Apprenticeship, JobProfile);
            byte[] messageBody = CommonAction.ConvertObjectToByteArray(entryRequirementsClassification);
            Message message = CommonAction.CreateServiceBusMessage(JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.ApprenticeshipEntryRequirements);
            await Topic.SendAsync(message);
            await Task.Delay(5000);
            Response<HowToBecomeAPIResponse> howToBecomeResponse = await CommonAction.ExecuteGetRequest<HowToBecomeAPIResponse>(Settings.APIConfig.EndpointBaseUrl.JSONContent.Replace("{id}", JobProfile.JobProfileId));
            Assert.AreEqual(entryRequirementsClassification.Title, howToBecomeResponse.Data.entryRoutes.apprenticeship.entryRequirementPreface);
        }
    }
}