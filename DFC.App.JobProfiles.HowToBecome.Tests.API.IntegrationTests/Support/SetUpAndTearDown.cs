using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.JobProfile;
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
            this.CommonAction.InitialiseAppSettings();
            this.Topic = new Topic(Settings.ServiceBusConfig.Endpoint);

            this.JobProfile = this.CommonAction.GenerateJobProfileContentType();

            RouteEntry universityRouteEntry = this.CommonAction.GenerateRouteEntryForRouteEntryType(RouteEntryType.University);
            EntryRequirement universityEntryRequirementSection = this.CommonAction.GenerateEntryRequirementSection(RouteEntryType.University);
            universityRouteEntry.EntryRequirements.Add(universityEntryRequirementSection);
            MoreInformationLink universityMoreInformationLinkSection = this.CommonAction.GenerateMoreInformationLinkSection(RouteEntryType.University);
            universityRouteEntry.MoreInformationLinks.Add(universityMoreInformationLinkSection);
            this.JobProfile.HowToBecomeData.RouteEntries.Add(universityRouteEntry);

            RouteEntry collegeRouteEntry = this.CommonAction.GenerateRouteEntryForRouteEntryType(RouteEntryType.College);
            EntryRequirement collegeEntryRequirementSection = this.CommonAction.GenerateEntryRequirementSection(RouteEntryType.College);
            collegeRouteEntry.EntryRequirements.Add(collegeEntryRequirementSection);
            MoreInformationLink collegeMoreInformationLinkSection = this.CommonAction.GenerateMoreInformationLinkSection(RouteEntryType.College);
            collegeRouteEntry.MoreInformationLinks.Add(collegeMoreInformationLinkSection);
            this.JobProfile.HowToBecomeData.RouteEntries.Add(collegeRouteEntry);

            RouteEntry apprenticeshipRouteEntry = this.CommonAction.GenerateRouteEntryForRouteEntryType(RouteEntryType.Apprenticeship);
            EntryRequirement apprenticeshipEntryRequirementSection = this.CommonAction.GenerateEntryRequirementSection(RouteEntryType.Apprenticeship);
            apprenticeshipRouteEntry.EntryRequirements.Add(apprenticeshipEntryRequirementSection);
            MoreInformationLink apprenticeshipMoreInformationLinkSection = this.CommonAction.GenerateMoreInformationLinkSection(RouteEntryType.Apprenticeship);
            apprenticeshipRouteEntry.MoreInformationLinks.Add(apprenticeshipMoreInformationLinkSection);
            this.JobProfile.HowToBecomeData.RouteEntries.Add(apprenticeshipRouteEntry);

            Registration registrationsSection = this.CommonAction.GenerateRegistrationsSection();
            this.JobProfile.HowToBecomeData.Registrations.Add(registrationsSection);

            byte[] messageBody = this.CommonAction.ConvertObjectToByteArray(this.JobProfile);
            Message message = this.CommonAction.CreateServiceBusMessage(this.JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.JobProfile);
            await this.Topic.SendAsync(message).ConfigureAwait(true);
            await Task.Delay(5000).ConfigureAwait(true);
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await this.CommonAction.DeleteJobProfile(this.Topic, this.JobProfile).ConfigureAwait(true);
        }
    }
}
