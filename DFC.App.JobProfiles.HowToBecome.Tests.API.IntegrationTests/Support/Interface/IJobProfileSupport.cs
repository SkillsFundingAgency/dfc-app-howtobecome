using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.JobProfile;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.Interface
{
    public interface IJobProfileSupport
    {
        JobProfileContentType GenerateJobProfileContentType();

        Task DeleteJobProfile(Topic topic, JobProfileContentType jobProfile);
    }
}
