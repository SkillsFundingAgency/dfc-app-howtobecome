using DFC.Api.JobProfiles.Common.APISupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using HtmlAgilityPack;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Test.HTML.Update.EntryRequirement
{
    public class HTML_Update_EntryRequirement : SetUpAndTearDown
    {
        //[Test]
        //public async Task HTML_Update_EntryRequirement_University()
        //{
        //    string newEntryRequirementText = "new entry requirement text for university";
        //    EntryRequirementMessageBody EntryRequirementMessageBody = CommonAction.CreateEntryRequirementMessageBody(UniversityRouteEntry.EntryRequirements[0].Id, JobProfileId, newEntryRequirementText);
        //    await CommonAction.UpdateEntryRequirementForRequirementType(Topic, EntryRequirementMessageBody, RouteEntryType.University);
        //    await Task.Delay(5000);
        //    Response<HtmlDocument> howToBecomeResponse = await CommonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HTMLContent.Replace("{canonicalName}", CanonicalName));
        //    Dictionary<RouteEntryType, HowToBecomeRouteEntry> observedRouteEntries = CommonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
        //    Assert.AreEqual(1, observedRouteEntries[RouteEntryType.University].EntryRequirements.Count);
        //    Assert.AreEqual(newEntryRequirementText, observedRouteEntries[RouteEntryType.University].EntryRequirements[0].Info);
        //}

        //[Test]
        //public async Task HTML_Update_EntryRequirement_College()
        //{
        //    string newEntryRequirementText = "new entry requirement text for college";
        //    CommonAction commonAction = new CommonAction();
        //    EntryRequirementMessageBody updateEntryRequirement = commonAction.CreateEntryRequirementMessageBody(CollegeRouteEntry.EntryRequirements[0].Id, JobProfileId, newEntryRequirementText);
        //    await commonAction.UpdateEntryRequirementForRequirementType(Topic, updateEntryRequirement, RouteEntryType.College);
        //    await Task.Delay(5000);
        //    Response<HtmlDocument> howToBecomeResponse = await commonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HTMLContent.Replace("{canonicalName}", CanonicalName));
        //    Dictionary<RouteEntryType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
        //    Assert.AreEqual(1, observedRouteEntries[RouteEntryType.College].EntryRequirements.Count);
        //    Assert.AreEqual(newEntryRequirementText, observedRouteEntries[RouteEntryType.College].EntryRequirements[0].Info);
        //}

        //[Test]
        //public async Task HTML_Update_EntryRequirement_Apprenticeship()
        //{
        //    string newEntryRequirementText = "new entry requirement text for apprenticeship";
        //    CommonAction commonAction = new CommonAction();
        //    EntryRequirementMessageBody updateEntryRequirement = commonAction.CreateEntryRequirementMessageBody(ApprenticeshipRouteEntry.EntryRequirements[0].Id, JobProfileId, newEntryRequirementText);
        //    await commonAction.UpdateEntryRequirementForRequirementType(Topic, updateEntryRequirement, RouteEntryType.Apprenticeship);
        //    await Task.Delay(5000);
        //    Response<HtmlDocument> howToBecomeResponse = await commonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HTMLContent.Replace("{canonicalName}", CanonicalName));
        //    Dictionary<RouteEntryType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
        //    Assert.AreEqual(1, observedRouteEntries[RouteEntryType.Apprenticeship].EntryRequirements.Count);
        //    Assert.AreEqual(newEntryRequirementText, observedRouteEntries[RouteEntryType.Apprenticeship].EntryRequirements[0].Info);
        //}
    }
}