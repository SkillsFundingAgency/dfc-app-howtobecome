using AutoMapper;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels;
using DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Services;
using FakeItEasy;
using Xunit;

namespace DFC.App.JobProfiles.HowToBecome.MFA.UnitTests.Services
{
    [Trait("Messaging Function", "Mapping Service Tests")]
    public class MappingServiceTests
    {
        private readonly IMapper mapper;
        private readonly IMappingService mappingService;

        public MappingServiceTests()
        {
            mapper = A.Fake<IMapper>();

            mappingService = new MappingService(mapper);
        }

        [Fact]
        public void ProcessAsyncLinkTestReturnsOk()
        {
            // arrange
            const string message = "{}";
            const long sequenceNumber = 1;
            var expectedResult = A.Fake<HowToBecomeSegmentModel>();

            A.CallTo(() => mapper.Map<HowToBecomeSegmentModel>(A<JobProfileMessage>.Ignored)).Returns(expectedResult);

            // act
            var result = mappingService.MapToSegmentModel(message, sequenceNumber);

            // assert
            A.CallTo(() => mapper.Map<HowToBecomeSegmentModel>(A<JobProfileMessage>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }
    }
}
