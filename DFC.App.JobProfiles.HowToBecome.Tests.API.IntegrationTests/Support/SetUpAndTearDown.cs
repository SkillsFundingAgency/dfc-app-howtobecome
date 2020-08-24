using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.Api.JobProfiles.IntegrationTests.Model.Support;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.ContentType;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.AzureServiceBus;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.AzureServiceBus.ServiceBusFactory;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support
{
    public class SetUpAndTearDown
    {
        protected ServiceBusSupport ServiceBus { get; set; }

        protected AppSettings AppSettings { get; set; }

        protected JobProfileContentType WakeUpJobProfile { get; set; }

        protected JobProfileContentType JobProfile { get; set; }

        protected CommonAction CommonAction { get; } = new CommonAction();

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            // Get settings from appsettings
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            this.AppSettings = configuration.Get<AppSettings>();
            this.ServiceBus = new ServiceBusSupport(new TopicClientFactory(), this.AppSettings);

            // Send wake up job profile
            this.WakeUpJobProfile = this.CommonAction.GetResource<JobProfileContentType>("JobProfileTemplate");
            this.WakeUpJobProfile.JobProfileId = Guid.NewGuid().ToString();
            this.WakeUpJobProfile.CanonicalName = this.CommonAction.RandomString(10).ToLowerInvariant();
            var jobProfileMessageBody = this.CommonAction.ConvertObjectToByteArray(this.WakeUpJobProfile);
            var message = new MessageFactory().Create(this.WakeUpJobProfile.JobProfileId, jobProfileMessageBody, "Published", "JobProfile");
            await this.ServiceBus.SendMessage(message).ConfigureAwait(false);
            await Task.Delay(TimeSpan.FromMinutes(this.AppSettings.DeploymentWaitInMinutes)).ConfigureAwait(true);

            // Generate a test job profile
            EntryRequirement universityEntryRequirementSection = new EntryRequirement()
            {
                Id = Guid.NewGuid().ToString(),
                Info = $"Automated {RouteEntryType.University} entry requirement",
                Title = $"Automated {RouteEntryType.University} entry requirement title",
            };

            MoreInformationLink universityMoreInformationLinkSection = new MoreInformationLink()
            {
                Id = Guid.NewGuid().ToString(),
                Text = $"Automated more information link for the {RouteEntryType.University} route entry type",
                Title = $"Automated more information title for the {RouteEntryType.University} route entry type",
                Url = new Uri($"https://{this.CommonAction.RandomString(10)}.com"),
            };

            RouteEntry universityRouteEntry = new RouteEntry()
            {
                RouteName = (int)RouteEntryType.University,
                EntryRequirements = new List<EntryRequirement>() { universityEntryRequirementSection },
                MoreInformationLinks = new List<MoreInformationLink>() { universityMoreInformationLinkSection },
                FurtherRouteInformation = $"Automated further information for the {RouteEntryType.University} route entry type",
                RouteRequirement = $"Automated route requirement for the {RouteEntryType.University} route entry type",
                RouteSubjects = $"Automated route subjects for the {RouteEntryType.University} route entry type",
            };

            EntryRequirement collegeEntryRequirementSection = new EntryRequirement()
            {
                Id = Guid.NewGuid().ToString(),
                Info = $"Automated {RouteEntryType.College} entry requirement",
                Title = $"Automated {RouteEntryType.College} entry requirement title",
            };

            MoreInformationLink collegeMoreInformationLinkSection = new MoreInformationLink()
            {
                Id = Guid.NewGuid().ToString(),
                Text = $"Automated more information link for the {RouteEntryType.College} route entry type",
                Title = $"Automated more information title for the {RouteEntryType.College} route entry type",
                Url = new Uri($"https://{this.CommonAction.RandomString(10)}.com"),
            };

            RouteEntry collegeRouteEntry = new RouteEntry()
            {
                RouteName = (int)RouteEntryType.College,
                EntryRequirements = new List<EntryRequirement>() { collegeEntryRequirementSection },
                MoreInformationLinks = new List<MoreInformationLink>() { collegeMoreInformationLinkSection },
                FurtherRouteInformation = $"Automated further information for the {RouteEntryType.College} route entry type",
                RouteRequirement = $"Automated route requirement for the {RouteEntryType.College} route entry type",
                RouteSubjects = $"Automated route subjects for the {RouteEntryType.College} route entry type",
            };

            EntryRequirement apprenticeshipEntryRequirementSection = new EntryRequirement()
            {
                Id = Guid.NewGuid().ToString(),
                Info = $"Automated {RouteEntryType.Apprenticeship} entry requirement",
                Title = $"Automated {RouteEntryType.Apprenticeship} entry requirement title",
            };

            MoreInformationLink apprenticeshipMoreInformationLinkSection = new MoreInformationLink()
            {
                Id = Guid.NewGuid().ToString(),
                Text = $"Automated more information link for the {RouteEntryType.Apprenticeship} route entry type",
                Title = $"Automated more information title for the {RouteEntryType.Apprenticeship} route entry type",
                Url = new Uri($"https://{this.CommonAction.RandomString(10)}.com"),
            };

            RouteEntry apprenticeshipRouteEntry = new RouteEntry()
            {
                RouteName = (int)RouteEntryType.Apprenticeship,
                EntryRequirements = new List<EntryRequirement>() { apprenticeshipEntryRequirementSection },
                MoreInformationLinks = new List<MoreInformationLink>() { apprenticeshipMoreInformationLinkSection },
                FurtherRouteInformation = $"Automated further information for the {RouteEntryType.Apprenticeship} route entry type",
                RouteRequirement = $"Automated route requirement for the {RouteEntryType.Apprenticeship} route entry type",
                RouteSubjects = $"Automated route subjects for the {RouteEntryType.Apprenticeship} route entry type",
            };

            Registration registrationsSection = new Registration()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Default registrations title",
                Info = "Default registrations info",
            };

            this.JobProfile = this.CommonAction.GetResource<JobProfileContentType>("JobProfileTemplate");
            this.JobProfile.JobProfileId = Guid.NewGuid().ToString();
            this.JobProfile.CanonicalName = this.CommonAction.RandomString(10).ToLowerInvariant();
            this.JobProfile.HowToBecomeData.RouteEntries.Add(universityRouteEntry);
            this.JobProfile.HowToBecomeData.RouteEntries.Add(collegeRouteEntry);
            this.JobProfile.HowToBecomeData.RouteEntries.Add(apprenticeshipRouteEntry);
            this.JobProfile.HowToBecomeData.Registrations.Add(registrationsSection);

            // Send job profile to the service bus
            jobProfileMessageBody = this.CommonAction.ConvertObjectToByteArray(this.JobProfile);
            message = new MessageFactory().Create(this.JobProfile.JobProfileId, jobProfileMessageBody, "Published", "JobProfile");
            await this.ServiceBus.SendMessage(message).ConfigureAwait(false);
            await Task.Delay(10000).ConfigureAwait(false);
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            // Delete wake up job profile
            var wakeUpJobProfileDelete = this.CommonAction.GetResource<JobProfileContentType>("JobProfileTemplate");
            wakeUpJobProfileDelete.JobProfileId = this.WakeUpJobProfile.JobProfileId;
            var messageBody = this.CommonAction.ConvertObjectToByteArray(wakeUpJobProfileDelete);
            var message = new MessageFactory().Create(this.WakeUpJobProfile.JobProfileId, messageBody, "Deleted", "JobProfile");
            await this.ServiceBus.SendMessage(message).ConfigureAwait(false);

            // Delete test job profile
            var jobProfileDelete = this.CommonAction.GetResource<JobProfileContentType>("JobProfileTemplate");
            jobProfileDelete.JobProfileId = this.JobProfile.JobProfileId;
            messageBody = this.CommonAction.ConvertObjectToByteArray(jobProfileDelete);
            message = new MessageFactory().Create(this.JobProfile.JobProfileId, messageBody, "Deleted", "JobProfile");
            await this.ServiceBus.SendMessage(message).ConfigureAwait(false);
        }
    }
}
