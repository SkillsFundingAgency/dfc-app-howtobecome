using DFC.Api.JobProfiles.Common.APISupport;
using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using HtmlAgilityPack;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Test.HTML.Update.RouteRequirement
{
    public class HTML_Update_RouteRequirement : SetUpAndTearDown
    {
        [Test]
        public async Task JobProfile_HowToBecome_UniversityEntryRequirements()
        {
            EntryRequirementsClassification entryRequirementsClassification = CommonAction.GenerateEntryRequirementsClassificationForJobProfile(RouteEntryType.University, JobProfile);
            byte[] messageBody = CommonAction.ConvertObjectToByteArray(entryRequirementsClassification);
            Message message = CommonAction.CreateServiceBusMessage(JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.JobProfile);
            await Topic.SendAsync(message);
            await Task.Delay(5000);
            Response<HowToBecomeAPIResponse> howToBecomeResponse = await CommonAction.ExecuteGetRequestWithJsonResponse<HowToBecomeAPIResponse>(Settings.APIConfig.EndpointBaseUrl.JSONContent.Replace("{id}", JobProfile.JobProfileId));
            Assert.AreEqual(entryRequirementsClassification.Title, howToBecomeResponse.Data.entryRoutes.university.entryRequirements[0]);
        }

        //[Test]
        //public async Task Html_RouteRequirementUpdate_College()
        //{
        //    string updatedRouteRequirement = "This is an updated value for the college route requirement";
        //    CommonAction commonAction = new CommonAction();
        //    UpdateRouteRequirement updateRouteRequirement = commonAction.GenerateRouteRequirementContentTypeForJobProfile(CollegeRouteRequirementId, updatedRouteRequirement);
        //    updateRouteRequirement.JobProfileId = JobProfileId.ToString();
        //    await commonAction.UpdateRouteRequirement(Topic, updateRouteRequirement, RouteEntryType.College);
        //    await Task.Delay(5000);
        //    Response<HtmlDocument> howToBecomeResponse = await commonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HTMLContent.Replace("{canonicalName}", CanonicalName));
        //    Dictionary<RouteEntryType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
        //    Assert.AreEqual(updatedRouteRequirement, observedRouteEntries[RouteEntryType.College].RouteRequirement);
        //}

        //[Test]
        //public async Task Html_RouteRequirementUpdate_Apprenticeship()
        //{
        //    string updatedRouteRequirement = "This is an updated value for the apprenticeship route requirement";
        //    CommonAction commonAction = new CommonAction();
        //    UpdateRouteRequirement updateRouteRequirement = commonAction.GenerateRouteRequirementContentTypeForJobProfile(ApprenticeshipsRouteRequirementId, updatedRouteRequirement);
        //    updateRouteRequirement.JobProfileId = JobProfileId.ToString();
        //    await commonAction.UpdateRouteRequirement(Topic, updateRouteRequirement, RouteEntryType.Apprenticeship);
        //    await Task.Delay(5000);
        //    Response<HtmlDocument> howToBecomeResponse = await commonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HTMLContent.Replace("{canonicalName}", CanonicalName));
        //    Dictionary<RouteEntryType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
        //    Assert.AreEqual(updatedRouteRequirement, observedRouteEntries[RouteEntryType.Apprenticeship].RouteRequirement);
        //}
    }
}