using DFC.Api.JobProfiles.Common.APISupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using HtmlAgilityPack;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Test.Update
{
    public class UpdateTest : Hook
    {
        [Test]
        [Ignore("This is blocked until DFC-11547 has been fixed")]
        public async Task Json()
        {
            Response<HowToBecomeRouteEntry> howToBecomeRouteEntry = await CommonAction.ExecuteGetRequestWithJsonResponse<HowToBecomeRouteEntry>(Settings.APIConfig.EndpointBaseUrl.HowToSegment + CanonicalName);
        }

        [Test]
        [Ignore("In development")]
        public async Task Html()
        {
            CommonAction commonAction = new CommonAction();
            RouteEntry updatedUniversityRouteEntry = commonAction.UpdateRouteEntryWithPrefix(UniversityRouteEntry, Settings.UpdatedRecordPrefix);
            RouteEntry updatedCollegeRouteEntry = commonAction.UpdateRouteEntryWithPrefix(CollegeRouteEntry, Settings.UpdatedRecordPrefix);
            RouteEntry updatedApprenticeshipRouteEntry = commonAction.UpdateRouteEntryWithPrefix(ApprenticeshipRouteEntry, Settings.UpdatedRecordPrefix);
            await CommonAction.UpdateJobProfileWithId(Topic, JobProfileId, CanonicalName, new RouteEntry[] { updatedUniversityRouteEntry, updatedCollegeRouteEntry, updatedApprenticeshipRouteEntry });
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

        [Test]
        public async Task Html_RouteRequirementUpdate_University()
        {
            string updatedRouteRequirement = "This is an updated value for the university route requirement";
            CommonAction commonAction = new CommonAction();
            UpdateRouteRequirement updateRouteRequirement = commonAction.GenerateRouteRequirementUpdate(UniversityRouteRequirementId, updatedRouteRequirement);
            updateRouteRequirement.JobProfileId = JobProfileId.ToString();
            await commonAction.UpdateRouteRequirement(Topic, updateRouteRequirement, RequirementType.University);
            await Task.Delay(5000);
            Response<HtmlDocument> howToBecomeResponse = await CommonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HowToSegment + CanonicalName);
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
            Response<HtmlDocument> howToBecomeResponse = await CommonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HowToSegment + CanonicalName);
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
            Response<HtmlDocument> howToBecomeResponse = await CommonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HowToSegment + CanonicalName);
            Dictionary<RequirementType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
            Assert.AreEqual(updatedRouteRequirement, observedRouteEntries[RequirementType.Apprenticeship].RouteRequirement);
        }
    }
}