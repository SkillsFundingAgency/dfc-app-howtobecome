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

        [Test]
        public async Task Html_MoreInformationLinksUpdate_Apprenticeship()
        {
            string newLinkText = "new more information link text for apprenticeships";
            CommonAction commonAction = new CommonAction();
            UpdateMoreInformationLink updateMoreInformationLink = commonAction.GenerateMoreInformationLinkUpdate(ApprenticeshipRouteEntry.MoreInformationLinks[0].Id, JobProfileId, newLinkText);
            await commonAction.UpdateMoreInformationLinks(Topic, updateMoreInformationLink, RequirementType.Apprenticeship);
            await Task.Delay(5000);
            Response<HtmlDocument> howToBecomeResponse = await CommonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HowToSegment + CanonicalName);
            Dictionary<RequirementType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
            Assert.AreEqual(1, observedRouteEntries[RequirementType.Apprenticeship].MoreInformationLinks.Count);
            Assert.AreEqual(newLinkText, observedRouteEntries[RequirementType.Apprenticeship].MoreInformationLinks[0].Text);
        }

        [Test]
        public async Task Html_MoreInformationLinksUpdate_College()
        {
            string newLinkText = "new more information link text for college";
            CommonAction commonAction = new CommonAction();
            UpdateMoreInformationLink updateMoreInformationLink = commonAction.GenerateMoreInformationLinkUpdate(CollegeRouteEntry.MoreInformationLinks[0].Id, JobProfileId, newLinkText);
            await commonAction.UpdateMoreInformationLinks(Topic, updateMoreInformationLink, RequirementType.College);
            await Task.Delay(5000);
            Response<HtmlDocument> howToBecomeResponse = await CommonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HowToSegment + CanonicalName);
            Dictionary<RequirementType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
            Assert.AreEqual(1, observedRouteEntries[RequirementType.College].MoreInformationLinks.Count);
            Assert.AreEqual(newLinkText, observedRouteEntries[RequirementType.College].MoreInformationLinks[0].Text);
        }

        [Test]
        public async Task Html_MoreInformationLinksUpdate_University()
        {
            string newLinkText = "new more information link text for apprenticeships";
            CommonAction commonAction = new CommonAction();
            UpdateMoreInformationLink updateMoreInformationLink = commonAction.GenerateMoreInformationLinkUpdate(UniversityRouteEntry.MoreInformationLinks[0].Id, JobProfileId, newLinkText);
            await commonAction.UpdateMoreInformationLinks(Topic, updateMoreInformationLink, RequirementType.University);
            await Task.Delay(5000);
            Response<HtmlDocument> howToBecomeResponse = await CommonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HowToSegment + CanonicalName);
            Dictionary<RequirementType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
            Assert.AreEqual(1, observedRouteEntries[RequirementType.University].MoreInformationLinks.Count);
            Assert.AreEqual(newLinkText, observedRouteEntries[RequirementType.University].MoreInformationLinks[0].Text);
        }

        [Test]
        public async Task Html_EntryRequirementUpdate_University()
        {
            string newEntryRequirementText = "new entry requirement text for university";
            CommonAction commonAction = new CommonAction();
            UpdateEntryRequirement updateEntryRequirement = commonAction.GenerateEntryRequirementUpdate(UniversityRouteEntry.EntryRequirements[0].Id, JobProfileId, newEntryRequirementText);
            await commonAction.UpdateEntryRequirement(Topic, updateEntryRequirement, RequirementType.University);
            await Task.Delay(5000);
            Response<HtmlDocument> howToBecomeResponse = await CommonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HowToSegment + CanonicalName);
            Dictionary<RequirementType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
            Assert.AreEqual(1, observedRouteEntries[RequirementType.University].EntryRequirements.Count);
            Assert.AreEqual(newEntryRequirementText, observedRouteEntries[RequirementType.University].EntryRequirements[0].Info);
        }

        [Test]
        public async Task Html_EntryRequirementUpdate_College()
        {
            string newEntryRequirementText = "new entry requirement text for college";
            CommonAction commonAction = new CommonAction();
            UpdateEntryRequirement updateEntryRequirement = commonAction.GenerateEntryRequirementUpdate(CollegeRouteEntry.EntryRequirements[0].Id, JobProfileId, newEntryRequirementText);
            await commonAction.UpdateEntryRequirement(Topic, updateEntryRequirement, RequirementType.College);
            await Task.Delay(5000);
            Response<HtmlDocument> howToBecomeResponse = await CommonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HowToSegment + CanonicalName);
            Dictionary<RequirementType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
            Assert.AreEqual(1, observedRouteEntries[RequirementType.College].EntryRequirements.Count);
            Assert.AreEqual(newEntryRequirementText, observedRouteEntries[RequirementType.College].EntryRequirements[0].Info);
        }

        [Test]
        public async Task Html_EntryRequirementUpdate_Apprenticeship()
        {
            string newEntryRequirementText = "new entry requirement text for apprenticeship";
            CommonAction commonAction = new CommonAction();
            UpdateEntryRequirement updateEntryRequirement = commonAction.GenerateEntryRequirementUpdate(ApprenticeshipRouteEntry.EntryRequirements[0].Id, JobProfileId, newEntryRequirementText);
            await commonAction.UpdateEntryRequirement(Topic, updateEntryRequirement, RequirementType.Apprenticeship);
            await Task.Delay(5000);
            Response<HtmlDocument> howToBecomeResponse = await CommonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HowToSegment + CanonicalName);
            Dictionary<RequirementType, HowToBecomeRouteEntry> observedRouteEntries = commonAction.GetRouteEntriesFromHtmlResponse(howToBecomeResponse);
            Assert.AreEqual(1, observedRouteEntries[RequirementType.Apprenticeship].EntryRequirements.Count);
            Assert.AreEqual(newEntryRequirementText, observedRouteEntries[RequirementType.Apprenticeship].EntryRequirements[0].Info);
        }

        [Test]
        public async Task Html_Registration_Apprenticeship()
        {
            string newRegistrationValue = "<p>new registration text for apprenticeship</p>";
            CommonAction commonAction = new CommonAction();
            UpdateRegistration updateRegistration = commonAction.GenerateRegistrationUpdate(RegistrationId, JobProfileId, newRegistrationValue);
            await commonAction.UpdateRegistration(Topic, updateRegistration);
            await Task.Delay(5000);
            Response<HtmlDocument> howToBecomeResponse = await CommonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HowToSegment + CanonicalName);
            string observedRegistrationValue = howToBecomeResponse.Data.DocumentNode.SelectSingleNode("//section[@id='moreinfo']/div[@class='job-profile-subsection-content']/ul/li").InnerHtml;
            Assert.AreEqual(newRegistrationValue, observedRegistrationValue);
        }
    }
}