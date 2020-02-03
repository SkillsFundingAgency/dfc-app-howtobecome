using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using NUnit.Framework;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support
{
    public class SetUpAndTearDown
    {
        internal JobProfileContentType JobProfile { get; set; }
        internal CommonAction CommonAction { get; } = new CommonAction();
        internal Topic Topic { get; set; }

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            CommonAction.InitialiseAppSettings();
            Topic = new Topic(Settings.ServiceBusConfig.Endpoint);

            JobProfile = CommonAction.GenerateJobProfileContentType();

            RouteEntry universityRouteEntry = CommonAction.GenerateRouteEntryForRouteEntryType(RouteEntryType.University);
            EntryRequirement universityEntryRequirementSection = CommonAction.GenerateEntryRequirementSection(RouteEntryType.University);
            universityRouteEntry.EntryRequirements.Add(universityEntryRequirementSection);
            MoreInformationLink universityMoreInformationLinkSection = CommonAction.GenerateMoreInformationLinkSection(RouteEntryType.University);
            universityRouteEntry.MoreInformationLinks.Add(universityMoreInformationLinkSection);
            JobProfile.HowToBecomeData.RouteEntries.Add(universityRouteEntry);

            RouteEntry collegeRouteEntry = CommonAction.GenerateRouteEntryForRouteEntryType(RouteEntryType.College);
            EntryRequirement collegeEntryRequirementSection = CommonAction.GenerateEntryRequirementSection(RouteEntryType.College);
            collegeRouteEntry.EntryRequirements.Add(collegeEntryRequirementSection);
            MoreInformationLink collegeMoreInformationLinkSection = CommonAction.GenerateMoreInformationLinkSection(RouteEntryType.College);
            collegeRouteEntry.MoreInformationLinks.Add(collegeMoreInformationLinkSection);
            JobProfile.HowToBecomeData.RouteEntries.Add(collegeRouteEntry);

            RouteEntry apprenticeshipRouteEntry = CommonAction.GenerateRouteEntryForRouteEntryType(RouteEntryType.College);
            EntryRequirement apprenticeshipEntryRequirementSection = CommonAction.GenerateEntryRequirementSection(RouteEntryType.College);
            apprenticeshipRouteEntry.EntryRequirements.Add(apprenticeshipEntryRequirementSection);
            MoreInformationLink apprenticeshipMoreInformationLinkSection = CommonAction.GenerateMoreInformationLinkSection(RouteEntryType.College);
            apprenticeshipRouteEntry.MoreInformationLinks.Add(apprenticeshipMoreInformationLinkSection);
            JobProfile.HowToBecomeData.RouteEntries.Add(apprenticeshipRouteEntry);

            Registration registrationsSection = CommonAction.GenerateRegistrationsSection();
            JobProfile.HowToBecomeData.Registrations.Add(registrationsSection);

            byte[] messageBody = CommonAction.ConvertObjectToByteArray(JobProfile);
            Message message = CommonAction.CreateServiceBusMessage(JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.JobProfile);
            await Topic.SendAsync(message);
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await CommonAction.DeleteJobProfile(Topic, JobProfile);
        }
    }
}
