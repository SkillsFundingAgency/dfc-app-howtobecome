using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels;
using DFC.App.JobProfiles.HowToBecome.Repository.CosmosDb;
using FakeItEasy;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobProfiles.HowToBecome.SegmentService.UnitTests
{
    [Trait("Profile Service", "Update Tests")]
    public class SegmentServiceUpsertTests
    {
        private readonly ICosmosRepository<HowToBecomeSegmentModel> repository;
        private readonly IHowToBecomeSegmentService howToBecomeSegmentService;

        public SegmentServiceUpsertTests()
        {
            var jobProfileSegmentRefreshService = A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
            var mapper = A.Fake<AutoMapper.IMapper>();
            repository = A.Fake<ICosmosRepository<HowToBecomeSegmentModel>>();
            howToBecomeSegmentService = new HowToBecomeSegmentService(repository, jobProfileSegmentRefreshService, mapper);
        }

        [Fact]
        public async Task HowToBecomeSegmentServiceUpsertReturnsCreatedWhenDocumentCreated()
        {
            // arrange
            var howToBecomeSegmentModel = A.Fake<HowToBecomeSegmentModel>();
            var expectedResult = HttpStatusCode.Created;

            A.CallTo(() => repository.UpsertAsync(howToBecomeSegmentModel)).Returns(expectedResult);

            // act
            var result = await howToBecomeSegmentService.UpsertAsync(howToBecomeSegmentModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.UpsertAsync(howToBecomeSegmentModel)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task HowToBecomeSegmentServiceUpsertReturnsSuccessWhenDocumentReplaced()
        {
            // arrange
            var howToBecomeSegmentModel = A.Fake<HowToBecomeSegmentModel>();
            var expectedResult = HttpStatusCode.OK;

            A.CallTo(() => repository.UpsertAsync(howToBecomeSegmentModel)).Returns(expectedResult);

            // act
            var result = await howToBecomeSegmentService.UpsertAsync(howToBecomeSegmentModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.UpsertAsync(howToBecomeSegmentModel)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task HowToBecomeSegmentServiceUpsertReturnsArgumentNullExceptionWhenNullParamIsUsed()
        {
            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await howToBecomeSegmentService.UpsertAsync(null).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'howToBecomeSegmentModel')", exceptionResult.Message);
        }
    }
}