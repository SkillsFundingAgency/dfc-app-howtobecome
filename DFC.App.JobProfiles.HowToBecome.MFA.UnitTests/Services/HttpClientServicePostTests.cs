using DFC.App.CareerPath.Common.Contracts;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Models;
using DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Services;
using DFC.App.JobProfiles.HowToBecome.MFA.UnitTests.FakeHttpHandlers;
using DFC.Logger.AppInsights.Contracts;
using FakeItEasy;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobProfiles.HowToBecome.MFA.UnitTests.Services
{
    [Trait("Messaging Function", "HttpClientService Post Tests")]
    public class HttpClientServicePostTests
    {
        private readonly ILogService logService;
        private readonly ICorrelationIdProvider correlationIdProvider;
        private readonly SegmentClientOptions segmentClientOptions;

        public HttpClientServicePostTests()
        {
            logService = A.Fake<ILogService>();
            correlationIdProvider = A.Fake<ICorrelationIdProvider>();
            segmentClientOptions = new SegmentClientOptions
            {
                BaseAddress = new Uri("https://somewhere.com", UriKind.Absolute),
            };
        }

        [Fact]
        public async Task PostFullJobProfileAsyncReturnsOkStatusCodeForExistingId()
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            var httpResponse = new HttpResponseMessage { StatusCode = expectedResult };
            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = segmentClientOptions.BaseAddress };
            var httpClientService = new HttpClientService(segmentClientOptions, httpClient, logService, correlationIdProvider);

            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            // act
            var result = await httpClientService.PostFullJobProfileAsync(A.Fake<HowToBecomeSegmentModel>()).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }

        [Fact]
        public async Task PostFullJobProfileAsyncReturnsExceptionForBadStatus()
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.Forbidden;
            var httpResponse = new HttpResponseMessage { StatusCode = expectedResult, Content = new StringContent("bad Post") };
            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = segmentClientOptions.BaseAddress };
            var httpClientService = new HttpClientService(segmentClientOptions, httpClient, logService, correlationIdProvider);

            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            // act
            var exceptionResult = await Assert.ThrowsAsync<HttpRequestException>(async () => await httpClientService.PostFullJobProfileAsync(A.Fake<HowToBecomeSegmentModel>()).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal($"Response status code does not indicate success: {(int)expectedResult} ({expectedResult}).", exceptionResult.Message);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }
    }
}
