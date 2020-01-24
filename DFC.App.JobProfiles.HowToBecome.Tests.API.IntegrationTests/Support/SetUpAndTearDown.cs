using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support
{
    public class SetUpAndTearDown
    {
        public Topic Topic { get; set; }
        public Guid JobProfileId { get; set; }
        public Guid UniversityRouteRequirementId { get; set; }
        public Guid CollegeRouteRequirementId { get; set; }
        public Guid ApprenticeshipsRouteRequirementId { get; set; }
        public Guid RegistrationId { get; set; }
        public string CanonicalName { get; set; }
        public RouteEntry UniversityRouteEntry { get; set; }
        public RouteEntry CollegeRouteEntry { get; set; }
        public RouteEntry ApprenticeshipRouteEntry { get; set; }

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            CommonAction commonAction = new CommonAction();
            JobProfileId = Guid.NewGuid();
            UniversityRouteRequirementId = Guid.NewGuid();
            CollegeRouteRequirementId = Guid.NewGuid();
            ApprenticeshipsRouteRequirementId = Guid.NewGuid();
            RegistrationId = Guid.NewGuid();
            CanonicalName = commonAction.RandomString(10).ToLower();
            commonAction.InitialiseAppSettings();
            Topic = new Topic(Settings.ServiceBusConfig.Endpoint);

            UniversityRouteEntry = commonAction.CreateARouteEntry(RequirementType.University);
            CollegeRouteEntry = commonAction.CreateARouteEntry(RequirementType.College);
            ApprenticeshipRouteEntry = commonAction.CreateARouteEntry(RequirementType.Apprenticeship);

            await CommonAction.CreateJobProfile(Topic, JobProfileId, RegistrationId, CanonicalName, new List<RouteEntry>() { UniversityRouteEntry, CollegeRouteEntry, ApprenticeshipRouteEntry });
            await Task.Delay(5000);

            UpdateRouteRequirement collegeUpdateRouteRequirement = commonAction.GenerateRouteRequirementUpdate(CollegeRouteRequirementId, "Initial college value");
            collegeUpdateRouteRequirement.JobProfileId = JobProfileId.ToString();
            await commonAction.UpdateRouteRequirement(Topic, collegeUpdateRouteRequirement, RequirementType.College);

            UpdateRouteRequirement apprenticeshipsUpdateRouteRequirement = commonAction.GenerateRouteRequirementUpdate(ApprenticeshipsRouteRequirementId, "Initial apprenticeship value");
            apprenticeshipsUpdateRouteRequirement.JobProfileId = JobProfileId.ToString();
            await commonAction.UpdateRouteRequirement(Topic, apprenticeshipsUpdateRouteRequirement, RequirementType.Apprenticeship);

            UpdateRouteRequirement universityUpdateRouteRequirement = commonAction.GenerateRouteRequirementUpdate(UniversityRouteRequirementId, "Initial university value");
            universityUpdateRouteRequirement.JobProfileId = JobProfileId.ToString();
            await commonAction.UpdateRouteRequirement(Topic, universityUpdateRouteRequirement, RequirementType.University);

            UpdateRegistration updateRegistration = commonAction.GenerateRegistrationUpdate(RegistrationId, JobProfileId, "<p>Initial registration text</p>");
            await commonAction.UpdateRegistration(Topic, updateRegistration);
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await CommonAction.DeleteJobProfileWithId(Topic, JobProfileId);
        }
    }
}
