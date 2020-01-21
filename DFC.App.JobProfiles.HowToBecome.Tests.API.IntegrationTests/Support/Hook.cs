using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support
{
    public class Hook
    {
        public Topic Topic { get; set; }
        public Guid MessageId { get; set; }
        public string CanonicalName { get; set; }
        public RouteEntry UniversityRouteEntry { get; set; }
        public RouteEntry CollegeRouteEntry { get; set; }
        public RouteEntry ApprentishipRouteEntry { get; set; }

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            CommonAction commonAction = new CommonAction();
            MessageId = Guid.NewGuid();
            CanonicalName = CommonAction.RandomString(10).ToLower();
            CommonAction.InitialiseAppSettings();
            Topic = new Topic(Settings.ServiceBusConfig.Endpoint);
            UniversityRouteEntry = commonAction.CreateARouteEntry(RequirementType.University);
            CollegeRouteEntry = commonAction.CreateARouteEntry(RequirementType.College);
            ApprentishipRouteEntry = commonAction.CreateARouteEntry(RequirementType.Apprentiships);
            await CommonAction.CreateJobProfile(Topic, MessageId, CanonicalName, new List<RouteEntry>() { UniversityRouteEntry, CollegeRouteEntry, ApprentishipRouteEntry });
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await CommonAction.DeleteJobProfileWithId(Topic, MessageId);
        }
    }
}
