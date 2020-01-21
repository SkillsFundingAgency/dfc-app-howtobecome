using DFC.Api.JobProfiles.Common.APISupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using HtmlAgilityPack;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Test
{
    public class Tests : Hook
    {
        [Test]
        public async Task Json()
        {
            Response<HowToBecomeRouteEntry> howToBecomeRouteEntry = await CommonAction.ExecuteGetRequestWithJsonResponse<HowToBecomeRouteEntry>(Settings.APIConfig.EndpointBaseUrl.HowToSegment + CanonicalName);
        }

        [Test]
        public async Task Html()
        {
            CommonAction commonAction = new CommonAction();
            Response<HtmlDocument> howToBecomeResponse = await CommonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HowToSegment + CanonicalName);
            Dictionary<RequirementType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);

            Assert.AreEqual(3, observedRouteEntries.Count);
            Assert.AreEqual(UniversityRouteEntry.RouteSubjects, observedRouteEntries[RequirementType.University].RouteSubjects);
            Assert.AreEqual(UniversityRouteEntry.FurtherRouteInformation, observedRouteEntries[RequirementType.University].FurtherRouteInformation);
            Assert.AreEqual(UniversityRouteEntry.RouteRequirement, observedRouteEntries[RequirementType.University].RouteRequirement);
            Assert.AreEqual(3, observedRouteEntries[RequirementType.University].EntryRequirements.Count);
            Assert.AreEqual(UniversityRouteEntry.EntryRequirements[0].Info, observedRouteEntries[RequirementType.University].EntryRequirements[0].Info);
            Assert.AreEqual(UniversityRouteEntry.EntryRequirements[1].Info, observedRouteEntries[RequirementType.University].EntryRequirements[1].Info);
            Assert.AreEqual(UniversityRouteEntry.EntryRequirements[2].Info, observedRouteEntries[RequirementType.University].EntryRequirements[2].Info);
            Assert.AreEqual(3, observedRouteEntries[RequirementType.University].MoreInformationLinks.Count);
            Assert.AreEqual(UniversityRouteEntry.MoreInformationLinks[0].Text, observedRouteEntries[RequirementType.University].MoreInformationLinks[0].Text);
            Assert.AreEqual(UniversityRouteEntry.MoreInformationLinks[1].Text, observedRouteEntries[RequirementType.University].MoreInformationLinks[1].Text);
            Assert.AreEqual(UniversityRouteEntry.MoreInformationLinks[2].Text, observedRouteEntries[RequirementType.University].MoreInformationLinks[2].Text);
        }
    }
}