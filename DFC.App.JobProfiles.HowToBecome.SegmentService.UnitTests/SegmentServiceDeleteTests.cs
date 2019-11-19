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
    [Trait("Segment Service", "Delete Tests")]
    public class SegmentServiceDeleteTests
    {
        private readonly ICosmosRepository<HowToBecomeSegmentModel> repository;
        private readonly IHowToBecomeSegmentService howToBecomeSegmentService;

        private readonly Guid documentId = Guid.NewGuid();

        public SegmentServiceDeleteTests()
        {
            var mapper = A.Fake<AutoMapper.IMapper>();
            var jobProfileSegmentRefreshService = A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
            repository = A.Fake<ICosmosRepository<HowToBecomeSegmentModel>>();
            howToBecomeSegmentService = new HowToBecomeSegmentService(repository, jobProfileSegmentRefreshService, mapper);
        }

        [Fact]
        public async Task HowToBecomeSegmentServiceDeleteReturnsSuccessWhenSegmentDeleted()
        {
            // arrange
            A.CallTo(() => repository.DeleteAsync(documentId)).Returns(HttpStatusCode.NoContent);

            // act
            var result = await howToBecomeSegmentService.DeleteAsync(documentId).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.DeleteAsync(documentId)).MustHaveHappenedOnceExactly();
            Assert.True(result);
        }

        [Fact]
        public async Task HowToBecomeSegmentServiceDeleteReturnsFalseWhenDocumentNotFound()
        {
            // arrange
            A.CallTo(() => repository.DeleteAsync(documentId)).Returns(HttpStatusCode.NotFound);

            // act
            var result = await howToBecomeSegmentService.DeleteAsync(documentId).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.DeleteAsync(documentId)).MustHaveHappenedOnceExactly();
            Assert.False(result);
        }

        [Fact]
        public async Task HowToBecomeSegmentServiceDeleteReturnsFalseWhenAnyOtherStatus()
        {
            // arrange
            A.CallTo(() => repository.DeleteAsync(documentId)).Returns(HttpStatusCode.BadRequest);

            // act
            var result = await howToBecomeSegmentService.DeleteAsync(documentId).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.DeleteAsync(documentId)).MustHaveHappenedOnceExactly();
            Assert.False(result);
        }
    }
}