using DFC.Api.JobProfiles.IntegrationTests.Model.Support;
using DFC.Api.JobProfiles.IntegrationTests.Support.API;
using DFC.App.JobProfiles.HowToBecome.FunctionalTests.Model.APIResponse;
using DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.API;
using DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.API.RestFactory.Interfaces;
using FakeItEasy;
using RestSharp;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobProfiles.HowToBecome.TestFramework.UnitTests
{
    public class JobProfileAPITests
    {
        [Fact]
        public async Task JobProfileAPICallsOnRestClientExecute()
        {
            // Arrange
            var fakeRestClient = A.Fake<IRestClient>();
            var fakeRestRequest = A.Fake<IRestRequest>();
            var fakeRestClientFactory = A.Fake<IRestClientFactory>();
            var fakeRestRequestFactory = A.Fake<IRestRequestFactory>();
            A.CallTo(() => fakeRestClientFactory.Create(A<Uri>.Ignored)).Returns(fakeRestClient);
            A.CallTo(() => fakeRestRequestFactory.Create(A<string>.Ignored)).Returns(fakeRestRequest);
            var apiSettings = new APISettings() { Endpoint = A.Fake<Uri>() };
            var jobProfileApi = new JobProfileApi(fakeRestClientFactory, fakeRestRequestFactory, new AppSettings(), apiSettings);

            // Act
            await jobProfileApi.GetById<HowToBecomeAPIResponse>("fakeValue").ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeRestClient.Execute<HowToBecomeAPIResponse>(fakeRestRequest)).MustHaveHappenedOnceExactly();
        }
    }
}
