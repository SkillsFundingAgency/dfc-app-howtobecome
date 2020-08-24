using DFC.Api.JobProfiles.Common.APISupport;
using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.APIResponse;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using NUnit.Framework;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Test
{
    public class Registration : SetUpAndTearDown
    {
        //[Test]
        //public async Task JobProfileHowToBecomeRegistration()
        //{
        //    RegistrationsContentType registrationsContentType = this.CommonAction.GenerateRegistrationsContentTypeForJobProfile(this.JobProfile);
        //    byte[] messageBody = this.CommonAction.ConvertObjectToByteArray(registrationsContentType);
        //    Message message = this.CommonAction.CreateServiceBusMessage(this.JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.Registration);
        //    await this.Topic.SendAsync(message).ConfigureAwait(true);
        //    await Task.Delay(5000).ConfigureAwait(true);
        //    Response<HowToBecomeAPIResponse> howToBecomeResponse = await this.CommonAction.ExecuteGetRequest<HowToBecomeAPIResponse>(Settings.APIConfig.EndpointBaseUrl.Replace("{id}", this.JobProfile.JobProfileId, System.StringComparison.InvariantCultureIgnoreCase)).ConfigureAwait(true);
        //    Assert.AreEqual(registrationsContentType.Info, howToBecomeResponse.Data.MoreInformation.Registrations[0]);
        //}
    }
}