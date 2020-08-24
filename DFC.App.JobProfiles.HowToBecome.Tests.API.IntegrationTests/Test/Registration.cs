using DFC.Api.JobProfiles.IntegrationTests.Support.API;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.APIResponse;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.ContentType;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.API;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.API.RestFactory;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.AzureServiceBus.ServiceBusFactory;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Test
{
    public class Registration : SetUpAndTearDown
    {
        private JobProfileApi howToBecomeAPI;

        [SetUp]
        public void SetUp()
        {
            var apiSettings = new APISettings { Endpoint = new Uri(this.AppSettings.APIConfig.EndpointBaseUrl) };
            this.howToBecomeAPI = new JobProfileApi(new RestClientFactory(), new RestRequestFactory(), this.AppSettings, apiSettings);
        }

        [Test]
        public async Task JobProfileHowToBecomeRegistration()
        {
            RegistrationsContentType registrationsContentType = new RegistrationsContentType()
            {
                Id = this.JobProfile.HowToBecomeData.Registrations[0].Id,
                Info = "This is the upated info for the registrations record",
                JobProfileId = this.JobProfile.JobProfileId,
                JobProfileTitle = this.JobProfile.Title,
                Title = "This is the upated title for the registrations record",
            };

            var messageBody = this.CommonAction.ConvertObjectToByteArray(registrationsContentType);
            var message = new MessageFactory().Create(this.JobProfile.JobProfileId, messageBody, "Published", "Registration");
            await this.ServiceBus.SendMessage(message).ConfigureAwait(false);
            await Task.Delay(5000).ConfigureAwait(true);
            var response = await this.howToBecomeAPI.GetById<HowToBecomeAPIResponse>(this.JobProfile.JobProfileId).ConfigureAwait(true); 
            Assert.AreEqual(registrationsContentType.Info, response.Data.MoreInformation.Registrations[0]);
        }
    }
}