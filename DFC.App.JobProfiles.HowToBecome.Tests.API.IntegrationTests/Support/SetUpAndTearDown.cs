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
        internal CommonAction CommonAction { get; } = new CommonAction();
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
            JobProfileId = Guid.NewGuid();
            UniversityRouteRequirementId = Guid.NewGuid();
            CollegeRouteRequirementId = Guid.NewGuid();
            ApprenticeshipsRouteRequirementId = Guid.NewGuid();
            RegistrationId = Guid.NewGuid();
            CanonicalName = CommonAction.RandomString(10).ToLower();
            CommonAction.InitialiseAppSettings();
            Topic = new Topic(Settings.ServiceBusConfig.Endpoint);

            UniversityRouteEntry = CommonAction.CreateARouteEntry(RequirementType.University);
            CollegeRouteEntry = CommonAction.CreateARouteEntry(RequirementType.College);
            ApprenticeshipRouteEntry = CommonAction.CreateARouteEntry(RequirementType.Apprenticeship);

            await CommonAction.CreateJobProfile(Topic, JobProfileId, RegistrationId, CanonicalName, new List<RouteEntry>() { UniversityRouteEntry, CollegeRouteEntry, ApprenticeshipRouteEntry });
            await Task.Delay(5000);

            UpdateRouteRequirement collegeUpdateRouteRequirement = CommonAction.GenerateRouteRequirementUpdate(CollegeRouteRequirementId, "Initial college value");
            collegeUpdateRouteRequirement.JobProfileId = JobProfileId.ToString();
            await CommonAction.UpdateRouteRequirement(Topic, collegeUpdateRouteRequirement, RequirementType.College);

            UpdateRouteRequirement apprenticeshipsUpdateRouteRequirement = CommonAction.GenerateRouteRequirementUpdate(ApprenticeshipsRouteRequirementId, "Initial apprenticeship value");
            apprenticeshipsUpdateRouteRequirement.JobProfileId = JobProfileId.ToString();
            await CommonAction.UpdateRouteRequirement(Topic, apprenticeshipsUpdateRouteRequirement, RequirementType.Apprenticeship);

            UpdateRouteRequirement universityUpdateRouteRequirement = CommonAction.GenerateRouteRequirementUpdate(UniversityRouteRequirementId, "Initial university value");
            universityUpdateRouteRequirement.JobProfileId = JobProfileId.ToString();
            await CommonAction.UpdateRouteRequirement(Topic, universityUpdateRouteRequirement, RequirementType.University);

            UpdateRegistration updateRegistration = CommonAction.GenerateRegistrationUpdate(RegistrationId, JobProfileId, "<p>Initial registration text</p>");
            await CommonAction.UpdateRegistration(Topic, updateRegistration);
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await CommonAction.DeleteJobProfileWithId(Topic, JobProfileId);
        }
    }
}
