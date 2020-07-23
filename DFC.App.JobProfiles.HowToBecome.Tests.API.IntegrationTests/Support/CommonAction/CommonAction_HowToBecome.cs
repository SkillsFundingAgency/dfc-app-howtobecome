using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.JobProfile;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support
{
    internal partial class CommonAction : IHowToBecomeSupport
    {
        public RouteEntry GenerateRouteEntryForRouteEntryType(EnumLibrary.RouteEntryType routeEntryType)
        {
            return new RouteEntry()
            {
                RouteName = (int)routeEntryType,
                EntryRequirements = new List<EntryRequirement>(),
                MoreInformationLinks = new List<MoreInformationLink>(),
                FurtherRouteInformation = $"Default further information for the {routeEntryType} route entry type",
                RouteRequirement = $"Default route requirement for the {routeEntryType} route entry type",
                RouteSubjects = $"Default route subjects for the {routeEntryType} route entry type",
            };
        }

        public MoreInformationLink GenerateMoreInformationLinkSection(EnumLibrary.RouteEntryType routeEntryType)
        {
            return new MoreInformationLink()
            {
                Id = Guid.NewGuid().ToString(),
                Text = $"Default more information link for the {routeEntryType} route entry type",
                Title = $"Default more information title for the {routeEntryType} route entry type",
                Url = $"https://{this.RandomString(10)}.com",
            };
        }

        public Registration GenerateRegistrationsSection()
        {
            return new Registration()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Default registrations title",
                Info = "Default registrations info",
            };
        }

        public EntryRequirement GenerateEntryRequirementSection(EnumLibrary.RouteEntryType entryRequirementType)
        {
            return new EntryRequirement()
            {
                Id = Guid.NewGuid().ToString(),
                Info = $"Default {entryRequirementType} entry requirement",
                Title = $"Default {entryRequirementType} entry requirement title",
            };
        }

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

        public LinksContentType GenerateLinksContentTypeForJobProfile(RouteEntryType routeEntryType, JobProfileContentType jobProfile)
        {
            RouteEntry routeEntry = jobProfile.HowToBecomeData.RouteEntries.Where(re => re.RouteName.Equals((int)routeEntryType)).FirstOrDefault();

            if (routeEntry == null)
            {
                throw new Exception($"Unable to find the route entry with route name {(int)routeEntryType}");
            }

            return new LinksContentType()
            {
                Id = routeEntry.MoreInformationLinks[0].Id,
                Text = "This is updated link text",
                Title = "This is an updated link title",
                Url = $"https://{this.RandomString(10)}.com/",
                JobProfileId = jobProfile.JobProfileId,
                JobProfileTitle = jobProfile.Title,
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
