using DFC.Api.JobProfiles.Common.APISupport;
using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.APIResponse;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.JobProfile;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Test
{
    public class MoreInformationLink : SetUpAndTearDown
    {
        [Test]
        public async Task JobProfileHowToBecomeUniversityLink()
        {
            LinksContentType linksContentType = this.CommonAction.GenerateLinksContentTypeForJobProfile(RouteEntryType.University, this.JobProfile);
            byte[] messageBody = this.CommonAction.ConvertObjectToByteArray(linksContentType);
            Message message = this.CommonAction.CreateServiceBusMessage(this.JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.UniversityLink);
            await this.Topic.SendAsync(message).ConfigureAwait(true);
            await Task.Delay(5000).ConfigureAwait(true);
            Response<HowToBecomeAPIResponse> howToBecomeResponse = await this.CommonAction.ExecuteGetRequest<HowToBecomeAPIResponse>(Settings.APIConfig.EndpointBaseUrl.Replace("{id}", this.JobProfile.JobProfileId, StringComparison.InvariantCultureIgnoreCase)).ConfigureAwait(true);
            Assert.AreEqual($"[{linksContentType.Text} | {linksContentType.Url.ToLower(CultureInfo.CurrentCulture)}]", howToBecomeResponse.Data.EntryRoutes.University.AdditionalInformation[0]);
        }

        [Test]
        public async Task JobProfileHowToBecomeCollegeLink()
        {
            LinksContentType linksContentType = this.CommonAction.GenerateLinksContentTypeForJobProfile(RouteEntryType.College, this.JobProfile);
            byte[] messageBody = this.CommonAction.ConvertObjectToByteArray(linksContentType);
            Message message = this.CommonAction.CreateServiceBusMessage(this.JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.CollegeLink);
            await this.Topic.SendAsync(message).ConfigureAwait(true);
            await Task.Delay(5000).ConfigureAwait(true);
            Response<HowToBecomeAPIResponse> howToBecomeResponse = await this.CommonAction.ExecuteGetRequest<HowToBecomeAPIResponse>(Settings.APIConfig.EndpointBaseUrl.Replace("{id}", this.JobProfile.JobProfileId, StringComparison.InvariantCultureIgnoreCase)).ConfigureAwait(true);
            Assert.AreEqual($"[{linksContentType.Text} | {linksContentType.Url.ToLower(CultureInfo.CurrentCulture)}]", howToBecomeResponse.Data.EntryRoutes.College.AdditionalInformation[0]);
        }

        [Test]
        public async Task JobProfileHowToBecomeApprenticeshipLink()
        {
            LinksContentType linksContentType = this.CommonAction.GenerateLinksContentTypeForJobProfile(RouteEntryType.Apprenticeship, this.JobProfile);
            byte[] messageBody = this.CommonAction.ConvertObjectToByteArray(linksContentType);
            Message message = this.CommonAction.CreateServiceBusMessage(this.JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.ApprenticeshipLink);
            await this.Topic.SendAsync(message).ConfigureAwait(true);
            await Task.Delay(5000).ConfigureAwait(true);
            Response<HowToBecomeAPIResponse> howToBecomeResponse = await this.CommonAction.ExecuteGetRequest<HowToBecomeAPIResponse>(Settings.APIConfig.EndpointBaseUrl.Replace("{id}", this.JobProfile.JobProfileId, StringComparison.InvariantCultureIgnoreCase)).ConfigureAwait(true);
            Assert.AreEqual($"[{linksContentType.Text} | {linksContentType.Url.ToLower(CultureInfo.CurrentCulture)}]", howToBecomeResponse.Data.EntryRoutes.Apprenticeship.AdditionalInformation[0]);
        }
    }
}