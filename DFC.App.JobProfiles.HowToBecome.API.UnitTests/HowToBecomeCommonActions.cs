using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.JobProfile;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DFC.App.JobProfiles.HowToBecome.API.UnitTests
{
    public class HowToBecomeCommonActions
    {
        [Test]
        public void UniversityRouteEntryHasRouteName2()
        {
            RouteEntry universityRouteEntry = new CommonAction().GenerateRouteEntryForRouteEntryType(EnumLibrary.RouteEntryType.University);
            Assert.AreEqual(2, universityRouteEntry.RouteName);
        }

        [Test]
        public void CollegeRouteEntryHasRouteName1()
        {
            RouteEntry collegeRouteEntry = new CommonAction().GenerateRouteEntryForRouteEntryType(EnumLibrary.RouteEntryType.College);
            Assert.AreEqual(1, collegeRouteEntry.RouteName);
        }

        [Test]
        public void ApprenRouteEntryHasRouteName0()
        {
            RouteEntry collegeRouteEntry = new CommonAction().GenerateRouteEntryForRouteEntryType(EnumLibrary.RouteEntryType.College);
            Assert.AreEqual(1, collegeRouteEntry.RouteName);
        }

        [Test]
        public void MoreInformationLinkSectionIsNotNull()
        {
            Assert.IsNotNull(new CommonAction().GenerateMoreInformationLinkSection(EnumLibrary.RouteEntryType.University));
        }

        [Test]
        public void RegistrationSectionIsNotNull()
        {
            Assert.IsNotNull(new CommonAction().GenerateRegistrationsSection());
        }

        [Test]
        public void EntryRequirementsClassificationHasCorrectIdentifiers()
        {
            JobProfileContentType jobProfileCotentType = new JobProfileContentType()
            {
                JobProfileId = Guid.NewGuid().ToString(),
                HowToBecomeData = new HowToBecomeData() { RouteEntries = new List<RouteEntry>() { new RouteEntry() { EntryRequirements = new List<EntryRequirement>() { new EntryRequirement() { Id = Guid.NewGuid().ToString() } } } } },
                Title = "Test title",
            };

            EntryRequirementsClassification erc = new CommonAction().GenerateEntryRequirementsClassificationForJobProfile(0, jobProfileCotentType);
            Assert.AreEqual(jobProfileCotentType.JobProfileId, erc.JobProfileId);
            Assert.AreEqual(jobProfileCotentType.Title, erc.JobProfileTitle);
            Assert.AreEqual(jobProfileCotentType.HowToBecomeData.RouteEntries[0].EntryRequirements[0].Id, erc.Id);
        }
    }
}