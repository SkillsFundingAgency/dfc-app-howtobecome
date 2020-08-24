using DFC.Api.JobProfiles.Common.APISupport;
using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.APIResponse;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.ContentType;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Test
{
    public class Requirement : SetUpAndTearDown
    {
        //[Test]
        //public async Task JobProfileHowToBecomeUniversityRequirement()
        //{
        //    RequirementContentType requirementContentType = this.CommonAction.GenerateRequirementClassificationForJobProfile(RouteEntryType.University, this.JobProfile);
        //    byte[] messageBody = this.CommonAction.ConvertObjectToByteArray(requirementContentType);
        //    Message message = this.CommonAction.CreateServiceBusMessage(this.JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.UniversityRequirement);
        //    await this.Topic.SendAsync(message).ConfigureAwait(true);
        //    await Task.Delay(5000).ConfigureAwait(true);
        //    Response<HowToBecomeAPIResponse> howToBecomeResponse = await this.CommonAction.ExecuteGetRequest<HowToBecomeAPIResponse>(Settings.APIConfig.EndpointBaseUrl.Replace("{id}", this.JobProfile.JobProfileId, StringComparison.InvariantCultureIgnoreCase)).ConfigureAwait(true);
        //    Assert.AreEqual(requirementContentType.Info, howToBecomeResponse.Data.EntryRoutes.University.EntryRequirements[0]);
        //}

        //[Test]
        //public async Task JobProfileHowToBecomeCollegeRequirement()
        //{
        //    RequirementContentType requirementContentType = this.CommonAction.GenerateRequirementClassificationForJobProfile(RouteEntryType.College, this.JobProfile);
        //    byte[] messageBody = this.CommonAction.ConvertObjectToByteArray(requirementContentType);
        //    Message message = this.CommonAction.CreateServiceBusMessage(this.JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.CollegeRequirement);
        //    await this.Topic.SendAsync(message).ConfigureAwait(true);
        //    await Task.Delay(5000).ConfigureAwait(true);
        //    Response<HowToBecomeAPIResponse> howToBecomeResponse = await this.CommonAction.ExecuteGetRequest<HowToBecomeAPIResponse>(Settings.APIConfig.EndpointBaseUrl.Replace("{id}", this.JobProfile.JobProfileId, StringComparison.InvariantCultureIgnoreCase)).ConfigureAwait(true);
        //    Assert.AreEqual(requirementContentType.Info, howToBecomeResponse.Data.EntryRoutes.College.EntryRequirements[0]);
        //}

        //[Test]
        //public async Task JobProfileHowToBecomeApprenticeshipRequirement()
        //{
        //    RequirementContentType requirementContentType = this.CommonAction.GenerateRequirementClassificationForJobProfile(RouteEntryType.Apprenticeship, this.JobProfile);
        //    byte[] messageBody = this.CommonAction.ConvertObjectToByteArray(requirementContentType);
        //    Message message = this.CommonAction.CreateServiceBusMessage(this.JobProfile.JobProfileId, messageBody, ContentType.JSON, ActionType.Published, CType.ApprenticeshipRequirement);
        //    await this.Topic.SendAsync(message).ConfigureAwait(true);
        //    await Task.Delay(5000).ConfigureAwait(true);
        //    Response<HowToBecomeAPIResponse> howToBecomeResponse = await this.CommonAction.ExecuteGetRequest<HowToBecomeAPIResponse>(Settings.APIConfig.EndpointBaseUrl.Replace("{id}", this.JobProfile.JobProfileId, StringComparison.InvariantCultureIgnoreCase)).ConfigureAwait(true);
        //    Assert.AreEqual(requirementContentType.Info, howToBecomeResponse.Data.EntryRoutes.Apprenticeship.EntryRequirements[0]);
        //}
    }
}