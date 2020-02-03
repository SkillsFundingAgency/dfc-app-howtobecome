using DFC.Api.JobProfiles.Common.APISupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using HtmlAgilityPack;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Test.HTML.Update.MoreInformationLink
{
    public class HTML_Update_MoreInformationLink : SetUpAndTearDown
    {
        //[Test]
        //public async Task HTML_Update_MoreInformationLink_Apprenticeship()
        //{
        //    string newLinkText = "new more information link text for apprenticeships";
        //    CommonAction commonAction = new CommonAction();
        //    UpdateMoreInformationLink updateMoreInformationLink = commonAction.GenerateMoreInformationLinkUpdate(ApprenticeshipRouteEntry.MoreInformationLinks[0].Id, JobProfileId, newLinkText);
        //    await commonAction.UpdateMoreInformationLinksForRequirementType(Topic, updateMoreInformationLink, RouteEntryType.Apprenticeship);
        //    await Task.Delay(5000);
        //    Response<HtmlDocument> howToBecomeResponse = await commonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HTMLContent.Replace("{canonicalName}", CanonicalName));
        //    Dictionary<RouteEntryType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
        //    Assert.AreEqual(1, observedRouteEntries[RouteEntryType.Apprenticeship].MoreInformationLinks.Count);
        //    Assert.AreEqual(newLinkText, observedRouteEntries[RouteEntryType.Apprenticeship].MoreInformationLinks[0].Text);
        //}

        //[Test]
        //public async Task HTML_Update_MoreInformationLink_College()
        //{
        //    string newLinkText = "new more information link text for college";
        //    CommonAction commonAction = new CommonAction();
        //    UpdateMoreInformationLink updateMoreInformationLink = commonAction.GenerateMoreInformationLinkUpdate(CollegeRouteEntry.MoreInformationLinks[0].Id, JobProfileId, newLinkText);
        //    await commonAction.UpdateMoreInformationLinksForRequirementType(Topic, updateMoreInformationLink, RouteEntryType.College);
        //    await Task.Delay(5000);
        //    Response<HtmlDocument> howToBecomeResponse = await commonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HTMLContent.Replace("{canonicalName}", CanonicalName));
        //    Dictionary<RouteEntryType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
        //    Assert.AreEqual(1, observedRouteEntries[RouteEntryType.College].MoreInformationLinks.Count);
        //    Assert.AreEqual(newLinkText, observedRouteEntries[RouteEntryType.College].MoreInformationLinks[0].Text);
        //}

        //[Test]
        //public async Task HTML_Update_MoreInformationLink_University()
        //{
        //    string newLinkText = "new more information link text for apprenticeships";
        //    CommonAction commonAction = new CommonAction();
        //    UpdateMoreInformationLink updateMoreInformationLink = commonAction.GenerateMoreInformationLinkUpdate(UniversityRouteEntry.MoreInformationLinks[0].Id, JobProfileId, newLinkText);
        //    await commonAction.UpdateMoreInformationLinksForRequirementType(Topic, updateMoreInformationLink, RouteEntryType.University);
        //    await Task.Delay(5000);
        //    Response<HtmlDocument> howToBecomeResponse = await commonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HTMLContent.Replace("{canonicalName}", CanonicalName));
        //    Dictionary<RouteEntryType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
        //    Assert.AreEqual(1, observedRouteEntries[RouteEntryType.University].MoreInformationLinks.Count);
        //    Assert.AreEqual(newLinkText, observedRouteEntries[RouteEntryType.University].MoreInformationLinks[0].Text);
        //}
    }
}