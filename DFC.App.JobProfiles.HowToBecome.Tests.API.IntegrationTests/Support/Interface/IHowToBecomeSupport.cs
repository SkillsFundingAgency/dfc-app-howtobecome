using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.JobProfile;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.Interface
{
    public interface IHowToBecomeSupport
    {
        RouteEntry GenerateRouteEntryForRouteEntryType(RouteEntryType routeEntryType);

        MoreInformationLink GenerateMoreInformationLinkSection(RouteEntryType routeEntryType);

        Registration GenerateRegistrationsSection();

        EntryRequirement GenerateEntryRequirementSection(RouteEntryType entryRequirementType);

        EntryRequirementsClassification GenerateEntryRequirementsClassificationForJobProfile(RouteEntryType routeEntryType, JobProfileContentType jobProfile);

        RegistrationsContentType GenerateRegistrationsContentTypeForJobProfile(JobProfileContentType jobProfile);

        LinksContentType GenerateLinksContentTypeForJobProfile(RouteEntryType routeEntryType, JobProfileContentType jobProfile);

        RequirementContentType GenerateRequirementClassificationForJobProfile(RouteEntryType routeEntryType, JobProfileContentType jobProfile);
    }
}
