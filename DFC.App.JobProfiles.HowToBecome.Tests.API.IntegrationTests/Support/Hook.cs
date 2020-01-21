using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support
{
    public class Hook
    {
        public Topic Topic { get; set; }
        public Guid MessageId { get; set; }
        public string CanonicalName { get; set; }

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            MessageId = Guid.NewGuid();
            CanonicalName = CommonAction.RandomString(10).ToLower();
            CommonAction.InitialiseAppSettings();
            Topic = new Topic(Settings.ServiceBusConfig.Endpoint);
            await CommonAction.CreateJobProfile(Topic, MessageId, CanonicalName);

            CommonAction commonAction = new CommonAction();
            RouteEntry universityRouteEntry = ResourceManager.GetResource<RouteEntry>("HowToBecomeRouteEntry");
            universityRouteEntry.RouteName = (int)RequirementType.University;
            commonAction.AddEntryRequirementToRouteEntry("Requirement one", universityRouteEntry);
            commonAction.AddEntryRequirementToRouteEntry("Requirement two", universityRouteEntry);
            commonAction.AddEntryRequirementToRouteEntry("Requirement three", universityRouteEntry);
            commonAction.AddMoreInformationLinkToRouteEntry("More information link one", universityRouteEntry);
            commonAction.AddMoreInformationLinkToRouteEntry("More information link two", universityRouteEntry);
            commonAction.AddMoreInformationLinkToRouteEntry("More information link three", universityRouteEntry);
            universityRouteEntry.RouteSubjects = "<div id='universityRouteSubjects'><p>This is a paragraph for route subjects.</p><ul><li>Listed item</li></ul></div>";
            universityRouteEntry.FurtherRouteInformation = "Automated further information";
            universityRouteEntry.RouteRequirement = "Automated requirement list";
            createInput.HowToBecomeData.RouteEntries.Add(universityRouteEntry);

            RouteEntry collegeRouteEntry = ResourceManager.GetResource<RouteEntry>("HowToBecomeRouteEntry");
            universityRouteEntry.RouteName = (int)RequirementType.College;
            commonAction.AddEntryRequirementToRouteEntry("Requirement one", collegeRouteEntry);
            commonAction.AddEntryRequirementToRouteEntry("Requirement two", collegeRouteEntry);
            commonAction.AddEntryRequirementToRouteEntry("Requirement three", collegeRouteEntry);
            commonAction.AddMoreInformationLinkToRouteEntry("More information link one", collegeRouteEntry);
            commonAction.AddMoreInformationLinkToRouteEntry("More information link two", collegeRouteEntry);
            commonAction.AddMoreInformationLinkToRouteEntry("More information link three", collegeRouteEntry);
            universityRouteEntry.RouteSubjects = "<div id='collegeRouteSubjects'><p>This is a paragraph for route subjects.</p><ul><li>Listed item</li></ul></div>";
            universityRouteEntry.FurtherRouteInformation = "Automated further information";
            universityRouteEntry.RouteRequirement = "Automated requirement list";
            createInput.HowToBecomeData.RouteEntries.Add(collegeRouteEntry);

            RouteEntry apprentishipRouteEntry = ResourceManager.GetResource<RouteEntry>("HowToBecomeRouteEntry");
            universityRouteEntry.RouteName = (int)RequirementType.Apprentiships;
            commonAction.AddEntryRequirementToRouteEntry("Requirement one", apprentishipRouteEntry);
            commonAction.AddEntryRequirementToRouteEntry("Requirement two", apprentishipRouteEntry);
            commonAction.AddEntryRequirementToRouteEntry("Requirement three", apprentishipRouteEntry);
            commonAction.AddMoreInformationLinkToRouteEntry("More information link one", apprentishipRouteEntry);
            commonAction.AddMoreInformationLinkToRouteEntry("More information link two", apprentishipRouteEntry);
            commonAction.AddMoreInformationLinkToRouteEntry("More information link three", apprentishipRouteEntry);
            universityRouteEntry.RouteSubjects = "<div id='apprentishipRouteSubjects'><p>This is a paragraph for route subjects.</p><ul><li>Listed item</li></ul></div>";
            universityRouteEntry.FurtherRouteInformation = "Automated further information";
            universityRouteEntry.RouteRequirement = "Automated requirement list";
            createInput.HowToBecomeData.RouteEntries.Add(apprentishipRouteEntry);
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await CommonAction.DeleteJobProfileWithId(Topic, MessageId);
        }
    }
}
