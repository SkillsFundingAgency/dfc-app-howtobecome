using DFC.Api.JobProfiles.Common.APISupport;
using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using NUnit.Framework;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Test
{
    public class MoreInformationLink : SetUpAndTearDown
    {
        [Test]
        public async Task JobProfile_HowToBecome_UniversityLink()
        {
            LinksContentType linksContentType = CommonAction.GenerateLinksContentTypeForJobProfile(RouteEntryType.University, JobProfile);
            byte[] messageBody = CommonAction.ConvertObjectToByteArray(linksContentType);
            Message message = CommonAction.CreateServiceBusMessage(JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.UniversityLink);
            await Topic.SendAsync(message);
            await Task.Delay(5000);
            Response<HowToBecomeAPIResponse> howToBecomeResponse = await CommonAction.ExecuteGetRequest<HowToBecomeAPIResponse>(Settings.APIConfig.EndpointBaseUrl.Replace("{id}", JobProfile.JobProfileId));
            Assert.AreEqual($"[{linksContentType.Text} | {linksContentType.Url.ToLower()}]", howToBecomeResponse.Data.entryRoutes.university.additionalInformation[0]);
        }

        [Test]
        public async Task JobProfile_HowToBecome_CollegeLink()
        {
            LinksContentType linksContentType = CommonAction.GenerateLinksContentTypeForJobProfile(RouteEntryType.College, JobProfile);
            byte[] messageBody = CommonAction.ConvertObjectToByteArray(linksContentType);
            Message message = CommonAction.CreateServiceBusMessage(JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.CollegeLink);
            await Topic.SendAsync(message);
            await Task.Delay(5000);
            Response<HowToBecomeAPIResponse> howToBecomeResponse = await CommonAction.ExecuteGetRequest<HowToBecomeAPIResponse>(Settings.APIConfig.EndpointBaseUrl.Replace("{id}", JobProfile.JobProfileId));
            Assert.AreEqual($"[{linksContentType.Text} | {linksContentType.Url.ToLower()}]", howToBecomeResponse.Data.entryRoutes.college.additionalInformation[0]);
        }

        [Test]
        public async Task JobProfile_HowToBecome_ApprenticeshipLink()
        {
            LinksContentType linksContentType = CommonAction.GenerateLinksContentTypeForJobProfile(RouteEntryType.Apprenticeship, JobProfile);
            byte[] messageBody = CommonAction.ConvertObjectToByteArray(linksContentType);
            Message message = CommonAction.CreateServiceBusMessage(JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.ApprenticeshipLink);
            await Topic.SendAsync(message);
            await Task.Delay(5000);
            Response<HowToBecomeAPIResponse> howToBecomeResponse = await CommonAction.ExecuteGetRequest<HowToBecomeAPIResponse>(Settings.APIConfig.EndpointBaseUrl.Replace("{id}", JobProfile.JobProfileId));
            Assert.AreEqual($"[{linksContentType.Text} | {linksContentType.Url.ToLower()}]", howToBecomeResponse.Data.entryRoutes.apprenticeship.additionalInformation[0]);
        }
    }
}