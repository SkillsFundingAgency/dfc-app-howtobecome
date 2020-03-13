using AutoMapper;
using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.Enums;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.PatchContentTypeModels;
using DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Services;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobProfiles.HowToBecome.MFA.UnitTests.Services
{
    [Trait("Messaging Function", "Message Processor Tests")]
    public class MessageProcessorTests
    {
        private readonly IMapper mapper;
        private readonly IHttpClientService httpClientService;
        private readonly IMappingService mappingService;
        private readonly IMessageProcessor messageProcessor;

        public MessageProcessorTests()
        {
            mapper = A.Fake<IMapper>();
            httpClientService = A.Fake<IHttpClientService>();
            mappingService = A.Fake<IMappingService>();

            messageProcessor = new MessageProcessor(mapper, httpClientService, mappingService);
        }

        public static IEnumerable<object[]> MessageContentTypeLinkData => new List<object[]>
        {
            new object[] { MessageContentType.ApprenticeshipLink },
            new object[] { MessageContentType.CollegeLink },
            new object[] { MessageContentType.UniversityLink },
        };

        public static IEnumerable<object[]> MessageContentTypeRequirementData => new List<object[]>
        {
            new object[] { MessageContentType.ApprenticeshipRequirement },
            new object[] { MessageContentType.CollegeRequirement },
            new object[] { MessageContentType.UniversityRequirement },
        };

        public static IEnumerable<object[]> MessageContentTypeEntryRequirementsData => new List<object[]>
        {
            new object[] { MessageContentType.ApprenticeshipEntryRequirements },
            new object[] { MessageContentType.CollegeEntryRequirements },
            new object[] { MessageContentType.UniversityEntryRequirements },
        };

        public static IEnumerable<object[]> MessageContentTypeRegistrationData => new List<object[]>
        {
            new object[] { MessageContentType.Registration },
        };

        [Theory]
        [MemberData(nameof(MessageContentTypeLinkData))]
        public async Task ProcessAsyncLinkTestReturnsOk(MessageContentType messageContentType)
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            const string message = "{}";
            const long sequenceNumber = 1;

            A.CallTo(() => mapper.Map<PatchLinksModel>(A<PatchLinksServiceBusModel>.Ignored)).Returns(A.Fake<PatchLinksModel>());
            A.CallTo(() => httpClientService.PatchAsync(A<PatchLinksModel>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            // act
            var result = await messageProcessor.ProcessAsync(message, sequenceNumber, messageContentType, MessageAction.Published).ConfigureAwait(false);

            // assert
            A.CallTo(() => mapper.Map<PatchLinksModel>(A<PatchLinksServiceBusModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => httpClientService.PatchAsync(A<PatchLinksModel>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [MemberData(nameof(MessageContentTypeRequirementData))]
        public async Task ProcessAsyncRequirementTestReturnsOk(MessageContentType messageContentType)
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            const string message = "{}";
            const long sequenceNumber = 1;

            A.CallTo(() => mapper.Map<PatchRequirementsModel>(A<PatchRequirementsServiceBusModel>.Ignored)).Returns(A.Fake<PatchRequirementsModel>());
            A.CallTo(() => httpClientService.PatchAsync(A<PatchRequirementsModel>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            // act
            var result = await messageProcessor.ProcessAsync(message, sequenceNumber, messageContentType, MessageAction.Published).ConfigureAwait(false);

            // assert
            A.CallTo(() => mapper.Map<PatchRequirementsModel>(A<PatchRequirementsServiceBusModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => httpClientService.PatchAsync(A<PatchRequirementsModel>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [MemberData(nameof(MessageContentTypeEntryRequirementsData))]
        public async Task ProcessAsyncEntryRequirementsTestReturnsOk(MessageContentType messageContentType)
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            const string message = "{}";
            const long sequenceNumber = 1;

            A.CallTo(() => mapper.Map<PatchSimpleClassificationModel>(A<PatchSimpleClassificationServiceBusModel>.Ignored)).Returns(A.Fake<PatchSimpleClassificationModel>());
            A.CallTo(() => httpClientService.PatchAsync(A<PatchSimpleClassificationModel>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            // act
            var result = await messageProcessor.ProcessAsync(message, sequenceNumber, messageContentType, MessageAction.Published).ConfigureAwait(false);

            // assert
            A.CallTo(() => mapper.Map<PatchSimpleClassificationModel>(A<PatchSimpleClassificationServiceBusModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => httpClientService.PatchAsync(A<PatchSimpleClassificationModel>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [MemberData(nameof(MessageContentTypeRegistrationData))]
        public async Task ProcessAsyncRegistrationTestReturnsOk(MessageContentType messageContentType)
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            const string message = "{}";
            const long sequenceNumber = 1;

            A.CallTo(() => mapper.Map<PatchRegistrationModel>(A<PatchRegistrationsServiceBusModel>.Ignored)).Returns(A.Fake<PatchRegistrationModel>());
            A.CallTo(() => httpClientService.PatchAsync(A<PatchRegistrationModel>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            // act
            var result = await messageProcessor.ProcessAsync(message, sequenceNumber, messageContentType, MessageAction.Published).ConfigureAwait(false);

            // assert
            A.CallTo(() => mapper.Map<PatchRegistrationModel>(A<PatchRegistrationsServiceBusModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => httpClientService.PatchAsync(A<PatchRegistrationModel>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task ProcessAsyncJobProfileCreatePublishedTestReturnsOk()
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.Created;
            const string message = "{}";
            const long sequenceNumber = 1;

            A.CallTo(() => mappingService.MapToSegmentModel(message, sequenceNumber)).Returns(A.Fake<HowToBecomeSegmentModel>());
            A.CallTo(() => httpClientService.PutFullJobProfileAsync(A<HowToBecomeSegmentModel>.Ignored)).Returns(expectedResult);

            // act
            var result = await messageProcessor.ProcessAsync(message, sequenceNumber, MessageContentType.JobProfile, MessageAction.Published).ConfigureAwait(false);

            // assert
            A.CallTo(() => mappingService.MapToSegmentModel(message, sequenceNumber)).MustHaveHappenedOnceExactly();
            A.CallTo(() => httpClientService.PutFullJobProfileAsync(A<HowToBecomeSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task ProcessAsyncJobProfileUpdatePublishedTestReturnsOk()
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            const string message = "{}";
            const long sequenceNumber = 1;

            A.CallTo(() => mappingService.MapToSegmentModel(message, sequenceNumber)).Returns(A.Fake<HowToBecomeSegmentModel>());
            A.CallTo(() => httpClientService.PutFullJobProfileAsync(A<HowToBecomeSegmentModel>.Ignored)).Returns(HttpStatusCode.NotFound);
            A.CallTo(() => httpClientService.PostFullJobProfileAsync(A<HowToBecomeSegmentModel>.Ignored)).Returns(expectedResult);

            // act
            var result = await messageProcessor.ProcessAsync(message, sequenceNumber, MessageContentType.JobProfile, MessageAction.Published).ConfigureAwait(false);

            // assert
            A.CallTo(() => mappingService.MapToSegmentModel(message, sequenceNumber)).MustHaveHappenedOnceExactly();
            A.CallTo(() => httpClientService.PutFullJobProfileAsync(A<HowToBecomeSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => httpClientService.PostFullJobProfileAsync(A<HowToBecomeSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task ProcessAsyncJobProfileDeletedTestReturnsOk()
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            const string message = "{}";
            const long sequenceNumber = 1;

            A.CallTo(() => mappingService.MapToSegmentModel(message, sequenceNumber)).Returns(A.Fake<HowToBecomeSegmentModel>());
            A.CallTo(() => httpClientService.DeleteAsync(A<Guid>.Ignored)).Returns(expectedResult);

            // act
            var result = await messageProcessor.ProcessAsync(message, sequenceNumber, MessageContentType.JobProfile, MessageAction.Deleted).ConfigureAwait(false);

            // assert
            A.CallTo(() => mappingService.MapToSegmentModel(message, sequenceNumber)).MustHaveHappenedOnceExactly();
            A.CallTo(() => httpClientService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task ProcessAsyncWithBadMessageContentTypeReturnsException()
        {
            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await messageProcessor.ProcessAsync(string.Empty, 1, (MessageContentType)(-1), MessageAction.Published).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Unexpected sitefinity content type '-1' (Parameter 'messageContentType')", exceptionResult.Message);
        }
    }
}
