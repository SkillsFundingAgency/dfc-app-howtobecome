using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.DraftSegmentService;
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
            var draftHowToBecomeSegmentService = A.Fake<IDraftHowToBecomeSegmentService>();
            repository = A.Fake<ICosmosRepository<HowToBecomeSegmentModel>>();
            howToBecomeSegmentService = new HowToBecomeSegmentService(repository, draftHowToBecomeSegmentService);
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
            Assert.Equal(expectedResult, result.ResponseStatusCode);
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
            Assert.Equal(expectedResult, result.ResponseStatusCode);
        }

        [Fact]
        public async Task HowToBecomeSegmentServiceUpsertReturnsArgumentNullExceptionWhenNullParamIsUsed()
        {
            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await howToBecomeSegmentService.UpsertAsync(null).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null.\r\nParameter name: howToBecomeSegmentModel", exceptionResult.Message);
        }
    }
}