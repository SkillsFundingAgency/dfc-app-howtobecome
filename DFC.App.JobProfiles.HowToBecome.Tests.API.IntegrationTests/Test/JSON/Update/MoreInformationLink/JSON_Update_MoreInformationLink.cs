using DFC.Api.JobProfiles.Common.APISupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using HtmlAgilityPack;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Test.JSON.Update.MoreInformationLink
{
    public class JSON_Update_MoreInformationLink : SetUpAndTearDown
    {
        [Test]
        public async Task Json_Update_MoreInformationLink_Apprenticeship()
        {
            string newLinkText = "new more information link text for apprenticeships";
            CommonAction commonAction = new CommonAction();
            UpdateMoreInformationLink updateMoreInformationLink = commonAction.GenerateMoreInformationLinkUpdate(ApprenticeshipRouteEntry.MoreInformationLinks[0].Id, JobProfileId, newLinkText);
            await commonAction.UpdateMoreInformationLinksForRequirementType(Topic, updateMoreInformationLink, RequirementType.Apprenticeship);
            await Task.Delay(5000);
            Response<HtmlDocument> howToBecomeResponse = await CommonAction.ExecuteGetRequestWithJsonResponse<HtmlDocument>(Settings.APIConfig.EndpointBaseUrl.JSONContent.Replace("{id}", JobProfileId.ToString()));
            Dictionary<RequirementType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
            Assert.AreEqual(1, observedRouteEntries[RequirementType.Apprenticeship].MoreInformationLinks.Count);
            Assert.AreEqual(newLinkText, observedRouteEntries[RequirementType.Apprenticeship].MoreInformationLinks[0].Text);
        }

        [Test]
        public async Task Json_Update_MoreInformationLink_College()
        {
            string newLinkText = "new more information link text for college";
            CommonAction commonAction = new CommonAction();
            UpdateMoreInformationLink updateMoreInformationLink = commonAction.GenerateMoreInformationLinkUpdate(CollegeRouteEntry.MoreInformationLinks[0].Id, JobProfileId, newLinkText);
            await commonAction.UpdateMoreInformationLinksForRequirementType(Topic, updateMoreInformationLink, RequirementType.College);
            await Task.Delay(5000);
            Response<HtmlDocument> howToBecomeResponse = await CommonAction.ExecuteGetRequestWithJsonResponse<HtmlDocument>(Settings.APIConfig.EndpointBaseUrl.JSONContent.Replace("{id}", JobProfileId.ToString()));
            Dictionary<RequirementType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
            Assert.AreEqual(1, observedRouteEntries[RequirementType.College].MoreInformationLinks.Count);
            Assert.AreEqual(newLinkText, observedRouteEntries[RequirementType.College].MoreInformationLinks[0].Text);
        }

        [Test]
        public async Task Json_Update_MoreInformationLink_University()
        {
            string newLinkText = "new more information link text for apprenticeships";
            CommonAction commonAction = new CommonAction();
            UpdateMoreInformationLink updateMoreInformationLink = commonAction.GenerateMoreInformationLinkUpdate(UniversityRouteEntry.MoreInformationLinks[0].Id, JobProfileId, newLinkText);
            await commonAction.UpdateMoreInformationLinksForRequirementType(Topic, updateMoreInformationLink, RequirementType.University);
            await Task.Delay(5000);
            Response<HtmlDocument> howToBecomeResponse = await CommonAction.ExecuteGetRequestWithJsonResponse<HtmlDocument>(Settings.APIConfig.EndpointBaseUrl.JSONContent.Replace("{id}", JobProfileId.ToString()));
            Dictionary<RequirementType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
            Assert.AreEqual(1, observedRouteEntries[RequirementType.University].MoreInformationLinks.Count);
            Assert.AreEqual(newLinkText, observedRouteEntries[RequirementType.University].MoreInformationLinks[0].Text);
        }
    }
}