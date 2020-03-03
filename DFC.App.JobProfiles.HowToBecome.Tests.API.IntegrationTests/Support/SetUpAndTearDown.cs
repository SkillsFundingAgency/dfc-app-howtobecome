using DFC.App.JobProfileOverview.Tests.IntegrationTests.API.Model.ContentType.JobProfile;
using DFC.App.JobProfileOverview.Tests.IntegrationTests.API.Model.Support;
using DFC.App.JobProfileOverview.Tests.IntegrationTests.API.Support.API;
using DFC.App.JobProfileOverview.Tests.IntegrationTests.API.Support.API.RestFactory;
using DFC.App.JobProfileOverview.Tests.IntegrationTests.API.Support.AzureServiceBus;
using DFC.App.JobProfileOverview.Tests.IntegrationTests.API.Support.AzureServiceBus.ServiceBusFactory;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DFC.App.JobProfileOverview.Tests.IntegrationTests.API.Support
{
    public class SetUpAndTearDown
    {
        internal CommonAction CommonAction { get; set; }

        internal AppSettings AppSettings { get; set; }

        internal JobProfileContentType JobProfile { get; set; }

        internal ServiceBusSupport ServiceBus { get; set; }

        internal JobProfileOverviewAPI API { get; set; }

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            this.AppSettings = configuration.Get<AppSettings>();
            this.CommonAction = new CommonAction();
            this.API = new JobProfileOverviewAPI(new RestClientFactory(), new RestRequestFactory(), this.AppSettings);
            string canonicalName = this.CommonAction.RandomString(10).ToUpperInvariant();
            this.JobProfile = this.CommonAction.GetResource<JobProfileContentType>("JobProfileContentType");
            this.JobProfile.JobProfileId = Guid.NewGuid().ToString();
            this.JobProfile.UrlName = canonicalName;
            this.JobProfile.CanonicalName = canonicalName;
            this.JobProfile.SocCodeData = this.CommonAction.GenerateSOCCodeJobProfileSection();
            this.JobProfile.WorkingHoursDetails = new List<WorkingHoursDetail>() { this.CommonAction.GenerateWorkingHoursDetailSection() };
            this.JobProfile.WorkingPattern = new List<WorkingPattern>() { this.CommonAction.GenerateWorkingPatternSection() };
            this.JobProfile.WorkingPatternDetails = new List<WorkingPatternDetail>() { this.CommonAction.GenerateWorkingPatternDetailsSection() };
            var jobProfileMessageBody = this.CommonAction.ConvertObjectToByteArray(this.JobProfile);
            this.ServiceBus = new ServiceBusSupport(new TopicClientFactory(), this.AppSettings);
            var message = new MessageFactory().Create(this.JobProfile.JobProfileId, jobProfileMessageBody, "Published", "JobProfile");
            await this.ServiceBus.SendMessage(message).ConfigureAwait(false);
            await Task.Delay(10000).ConfigureAwait(false);
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            var jobProfileDelete = this.CommonAction.GetResource<JobProfileContentType>("JobProfileDelete");
            var messageBody = this.CommonAction.ConvertObjectToByteArray(jobProfileDelete);
            var message = new MessageFactory().Create(this.JobProfile.JobProfileId, messageBody, "Deleted", "JobProfile");
            await this.ServiceBus.SendMessage(message).ConfigureAwait(false);
        }
    }
}
