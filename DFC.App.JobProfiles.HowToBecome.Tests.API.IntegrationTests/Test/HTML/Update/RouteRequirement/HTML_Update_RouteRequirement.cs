using DFC.Api.JobProfiles.Common.APISupport;
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
        public async Task Html_RouteRequirementUpdate_University()
        {
            string updatedRouteRequirement = "This is an updated value for the university route requirement";
            CommonAction commonAction = new CommonAction();
            UpdateRouteRequirement updateRouteRequirement = commonAction.GenerateRouteRequirementUpdate(UniversityRouteRequirementId, updatedRouteRequirement);
            updateRouteRequirement.JobProfileId = JobProfileId.ToString();
            await commonAction.UpdateRouteRequirement(Topic, updateRouteRequirement, RequirementType.University);
            await Task.Delay(5000);
            Response<HtmlDocument> howToBecomeResponse = await commonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HTMLContent.Replace("{canonicalName}", CanonicalName));
            Dictionary<RequirementType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
            Assert.AreEqual(updatedRouteRequirement, observedRouteEntries[RequirementType.University].RouteRequirement);
        }

        [Test]
        public async Task Html_RouteRequirementUpdate_College()
        {
            string updatedRouteRequirement = "This is an updated value for the college route requirement";
            CommonAction commonAction = new CommonAction();
            UpdateRouteRequirement updateRouteRequirement = commonAction.GenerateRouteRequirementUpdate(CollegeRouteRequirementId, updatedRouteRequirement);
            updateRouteRequirement.JobProfileId = JobProfileId.ToString();
            await commonAction.UpdateRouteRequirement(Topic, updateRouteRequirement, RequirementType.College);
            await Task.Delay(5000);
            Response<HtmlDocument> howToBecomeResponse = await commonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HTMLContent.Replace("{canonicalName}", CanonicalName));
            Dictionary<RequirementType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
            Assert.AreEqual(updatedRouteRequirement, observedRouteEntries[RequirementType.College].RouteRequirement);
        }

        [Test]
        public async Task Html_RouteRequirementUpdate_Apprenticeship()
        {
            string updatedRouteRequirement = "This is an updated value for the apprenticeship route requirement";
            CommonAction commonAction = new CommonAction();
            UpdateRouteRequirement updateRouteRequirement = commonAction.GenerateRouteRequirementUpdate(ApprenticeshipsRouteRequirementId, updatedRouteRequirement);
            updateRouteRequirement.JobProfileId = JobProfileId.ToString();
            await commonAction.UpdateRouteRequirement(Topic, updateRouteRequirement, RequirementType.Apprenticeship);
            await Task.Delay(5000);
            Response<HtmlDocument> howToBecomeResponse = await commonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HTMLContent.Replace("{canonicalName}", CanonicalName));
            Dictionary<RequirementType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
            Assert.AreEqual(updatedRouteRequirement, observedRouteEntries[RequirementType.Apprenticeship].RouteRequirement);
        }
    }
}