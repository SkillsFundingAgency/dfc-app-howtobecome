using DFC.Api.JobProfiles.Common.APISupport;
using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using NUnit.Framework;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Test
{
    public class Requirement : SetUpAndTearDown
    {
        [Test]
        public async Task JobProfile_HowToBecome_UniversityRequirement()
        {
            RequirementContentType requirementContentType = CommonAction.GenerateRequirementClassificationForJobProfile(RouteEntryType.University, JobProfile);
            byte[] messageBody = CommonAction.ConvertObjectToByteArray(requirementContentType);
            Message message = CommonAction.CreateServiceBusMessage(JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.UniversityRequirement);
            await Topic.SendAsync(message);
            await Task.Delay(5000);
            Response<HowToBecomeAPIResponse> howToBecomeResponse = await CommonAction.ExecuteGetRequest<HowToBecomeAPIResponse>(Settings.APIConfig.EndpointBaseUrl.Replace("{id}", JobProfile.JobProfileId));
            Assert.AreEqual(requirementContentType.Info, howToBecomeResponse.Data.entryRoutes.university.entryRequirements[0]);
        }

        [Test]
        public async Task JobProfile_HowToBecome_CollegeRequirement()
        {
            RequirementContentType requirementContentType = CommonAction.GenerateRequirementClassificationForJobProfile(RouteEntryType.College, JobProfile);
            byte[] messageBody = CommonAction.ConvertObjectToByteArray(requirementContentType);
            Message message = CommonAction.CreateServiceBusMessage(JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.CollegeRequirement);
            await Topic.SendAsync(message);
            await Task.Delay(5000);
            Response<HowToBecomeAPIResponse> howToBecomeResponse = await CommonAction.ExecuteGetRequest<HowToBecomeAPIResponse>(Settings.APIConfig.EndpointBaseUrl.Replace("{id}", JobProfile.JobProfileId));
            Assert.AreEqual(requirementContentType.Info, howToBecomeResponse.Data.entryRoutes.college.entryRequirements[0]);
        }

        [Test]
        public async Task JobProfile_HowToBecome_ApprenticeshipRequirement()
        {
            RequirementContentType requirementContentType = CommonAction.GenerateRequirementClassificationForJobProfile(RouteEntryType.Apprenticeship, JobProfile);
            byte[] messageBody = CommonAction.ConvertObjectToByteArray(requirementContentType);
            Message message = CommonAction.CreateServiceBusMessage(JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.ApprenticeshipRequirement);
            await Topic.SendAsync(message);
            await Task.Delay(5000);
            Response<HowToBecomeAPIResponse> howToBecomeResponse = await CommonAction.ExecuteGetRequest<HowToBecomeAPIResponse>(Settings.APIConfig.EndpointBaseUrl.Replace("{id}", JobProfile.JobProfileId));
            Assert.AreEqual(requirementContentType.Info, howToBecomeResponse.Data.entryRoutes.apprenticeship.entryRequirements[0]);
        }
    }
}