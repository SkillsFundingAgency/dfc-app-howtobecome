using DFC.Api.JobProfiles.Common.APISupport;
using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using NUnit.Framework;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Test
{
    public class Registration : SetUpAndTearDown
    {
        [Test]
        public async Task JobProfile_HowToBecome_Registration()
        {
            RegistrationsContentType registrationsContentType = CommonAction.GenerateRegistrationsContentTypeForJobProfile(JobProfile);
            byte[] messageBody = CommonAction.ConvertObjectToByteArray(registrationsContentType);
            Message message = CommonAction.CreateServiceBusMessage(JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.Registration);
            await Topic.SendAsync(message);
            await Task.Delay(5000);
            Response<HowToBecomeAPIResponse> howToBecomeResponse = await CommonAction.ExecuteGetRequest<HowToBecomeAPIResponse>(Settings.APIConfig.EndpointBaseUrl.Replace("{id}", JobProfile.JobProfileId));
            Assert.AreEqual(registrationsContentType.Info, howToBecomeResponse.Data.moreInformation.registrations[0]);
        }
    }
}