using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.JobProfile;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.Interface;
using System;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support
{
    public partial class CommonAction : IJobProfileSupport
    {
        public JobProfileContentType GenerateJobProfileContentType()
        {
            string canonicalName = this.GenerateUpperCaseRandomAlphaString(10);
            JobProfileContentType jobProfile = ResourceManager.GetResource<JobProfileContentType>("JobProfileContentType");
            jobProfile.JobProfileId = Guid.NewGuid().ToString();
            jobProfile.UrlName = canonicalName;
            jobProfile.CanonicalName = canonicalName;
            return jobProfile;
        }

        public async Task DeleteJobProfile(Topic topic, JobProfileContentType jobProfile)
        {
            JobProfileDeleteMessageBody messageBody = ResourceManager.GetResource<JobProfileDeleteMessageBody>("JobProfileDeleteMessageBody");
            messageBody.JobProfileId = jobProfile.JobProfileId;
            Message deleteMessage = this.CreateServiceBusMessage(jobProfile.JobProfileId, this.ConvertObjectToByteArray(messageBody), EnumLibrary.ContentType.JSON, ActionType.Deleted, CType.JobProfile);
            await topic.SendAsync(deleteMessage).ConfigureAwait(true);
        }
    }
}
