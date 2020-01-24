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
        [Test]
        public async Task HTML_Update_EntryRequirement_University()
        {
            string newEntryRequirementText = "new entry requirement text for university";
            CommonAction commonAction = new CommonAction();
            UpdateEntryRequirement updateEntryRequirement = commonAction.GenerateEntryRequirementUpdate(UniversityRouteEntry.EntryRequirements[0].Id, JobProfileId, newEntryRequirementText);
            await commonAction.UpdateEntryRequirement(Topic, updateEntryRequirement, RequirementType.University);
            await Task.Delay(5000);
            Response<HtmlDocument> howToBecomeResponse = await commonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HowToSegment + CanonicalName);
            Dictionary<RequirementType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
            Assert.AreEqual(1, observedRouteEntries[RequirementType.University].EntryRequirements.Count);
            Assert.AreEqual(newEntryRequirementText, observedRouteEntries[RequirementType.University].EntryRequirements[0].Info);
        }

        [Test]
        public async Task HTML_Update_EntryRequirement_College()
        {
            string newEntryRequirementText = "new entry requirement text for college";
            CommonAction commonAction = new CommonAction();
            UpdateEntryRequirement updateEntryRequirement = commonAction.GenerateEntryRequirementUpdate(CollegeRouteEntry.EntryRequirements[0].Id, JobProfileId, newEntryRequirementText);
            await commonAction.UpdateEntryRequirement(Topic, updateEntryRequirement, RequirementType.College);
            await Task.Delay(5000);
            Response<HtmlDocument> howToBecomeResponse = await commonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HowToSegment + CanonicalName);
            Dictionary<RequirementType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
            Assert.AreEqual(1, observedRouteEntries[RequirementType.College].EntryRequirements.Count);
            Assert.AreEqual(newEntryRequirementText, observedRouteEntries[RequirementType.College].EntryRequirements[0].Info);
        }

        [Test]
        public async Task HTML_Update_EntryRequirement_Apprenticeship()
        {
            string newEntryRequirementText = "new entry requirement text for apprenticeship";
            CommonAction commonAction = new CommonAction();
            UpdateEntryRequirement updateEntryRequirement = commonAction.GenerateEntryRequirementUpdate(ApprenticeshipRouteEntry.EntryRequirements[0].Id, JobProfileId, newEntryRequirementText);
            await commonAction.UpdateEntryRequirement(Topic, updateEntryRequirement, RequirementType.Apprenticeship);
            await Task.Delay(5000);
            Response<HtmlDocument> howToBecomeResponse = await commonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HowToSegment + CanonicalName);
            Dictionary<RequirementType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
            Assert.AreEqual(1, observedRouteEntries[RequirementType.Apprenticeship].EntryRequirements.Count);
            Assert.AreEqual(newEntryRequirementText, observedRouteEntries[RequirementType.Apprenticeship].EntryRequirements[0].Info);
        }
    }
}