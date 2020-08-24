using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.Classification;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.ContentType;
using System;
using System.Collections.Generic;
using System.Linq;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support
{
    public partial class CommonAction
    {
        public EntryRequirementsClassification GenerateEntryRequirementsClassificationForJobProfile(RouteEntryType routeEntryType, JobProfileContentType jobProfile)
        {
            return new EntryRequirementsClassification()
            {
                Id = jobProfile.HowToBecomeData.RouteEntries[(int)routeEntryType].EntryRequirements[0].Id,
                Description = $"This is an updated description for the entry requirement for the {routeEntryType.ToString()} route entry",
                Title = $"This is an updated title for the entry requirement for the {routeEntryType.ToString()} route entry",
                Url = $"https://{this.RandomString(10)}.com/",
                JobProfileId = jobProfile.JobProfileId,
                JobProfileTitle = jobProfile.Title,
            };
        }

        public RegistrationsContentType GenerateRegistrationsContentTypeForJobProfile(JobProfileContentType jobProfile)
        {
            return new RegistrationsContentType()
            {
                Id = jobProfile.HowToBecomeData.Registrations[0].Id,
                Info = "This is the upated info for the registrations record",
                JobProfileId = jobProfile.JobProfileId,
                JobProfileTitle = jobProfile.Title,
                Title = "This is the upated title for the registrations record",
            };
        }

        public RequirementContentType GenerateRequirementClassificationForJobProfile(RouteEntryType routeEntryType, JobProfileContentType jobProfile)
        {
            RouteEntry routeEntry = jobProfile.HowToBecomeData.RouteEntries.Where(re => re.RouteName.Equals((int)routeEntryType)).FirstOrDefault();

            if (routeEntry == null)
            {
                throw new Exception($"Unable to find the route entry with route name {(int)routeEntryType}");
            }

            return new RequirementContentType()
            {
                Id = routeEntry.EntryRequirements[0].Id,
                Info = "This is updated requirement info",
                Title = "This is an updated requirement title",
                JobProfileId = jobProfile.JobProfileId,
                JobProfileTitle = jobProfile.Title,
            };
        }
    }
}
