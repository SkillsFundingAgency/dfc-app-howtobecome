using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels;
using DFC.App.JobProfiles.HowToBecome.DraftSegmentService;
using DFC.App.JobProfiles.HowToBecome.Repository.CosmosDb;
using FakeItEasy;
using Xunit;

namespace DFC.App.JobProfiles.HowToBecome.SegmentService.UnitTests
{
    [Trait("Segment Service", "Ping / Health Tests")]
    public class SegmentServicePingTests
    {
        [Fact]
        public void HowToBecomeSegmentServicePingReturnsSuccess()
        {
            // arrange
            const bool expectedResult = true;
            var repository = A.Fake<ICosmosRepository<HowToBecomeSegmentModel>>();
            var jobProfileSegmentRefreshService = A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
            var mapper = A.Fake<AutoMapper.IMapper>();

            A.CallTo(() => repository.PingAsync()).Returns(expectedResult);

            var howToBecomeSegmentService = new HowToBecomeSegmentService(repository, A.Fake<IDraftHowToBecomeSegmentService>(), jobProfileSegmentRefreshService, mapper);

            // act
            var result = howToBecomeSegmentService.PingAsync().Result;

            // assert
            A.CallTo(() => repository.PingAsync()).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void HowToBecomeSegmentServicePingReturnsFalseWhenMissingRepository()
        {
            // arrange
            const bool expectedResult = false;
            var repository = A.Fake<ICosmosRepository<HowToBecomeSegmentModel>>();
            var jobProfileSegmentRefreshService = A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
            var mapper = A.Fake<AutoMapper.IMapper>();

            A.CallTo(() => repository.PingAsync()).Returns(expectedResult);

            var howToBecomeSegmentService = new HowToBecomeSegmentService(repository, A.Fake<IDraftHowToBecomeSegmentService>(), jobProfileSegmentRefreshService, mapper);

            // act
            var result = howToBecomeSegmentService.PingAsync().Result;

            // assert
            A.CallTo(() => repository.PingAsync()).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }
    }
}