using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.JobProfile;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.Interface
{
    internal interface IHowToBecomeSupport
    {
        RouteEntry GenerateRouteEntryForRouteEntryType(EnumLibrary.RouteEntryType routeEntryType);

        MoreInformationLink GenerateMoreInformationLinkSection(EnumLibrary.RouteEntryType routeEntryType);

        Registration GenerateRegistrationsSection();

        EntryRequirement GenerateEntryRequirementSection(EnumLibrary.RouteEntryType entryRequirementType);

        EntryRequirementsClassification GenerateEntryRequirementsClassificationForJobProfile(RouteEntryType routeEntryType, JobProfileContentType jobProfile);

        RegistrationsContentType GenerateRegistrationsContentTypeForJobProfile(JobProfileContentType jobProfile);

        LinksContentType GenerateLinksContentTypeForJobProfile(RouteEntryType routeEntryType, JobProfileContentType jobProfile);

        RequirementContentType GenerateRequirementClassificationForJobProfile(RouteEntryType routeEntryType, JobProfileContentType jobProfile);
    }
}
