﻿using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels;
using DFC.App.JobProfiles.HowToBecome.Repository.CosmosDb;
using FakeItEasy;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobProfiles.HowToBecome.SegmentService.UnitTests
{
    [Trait("Segment Service", "GetByName Tests")]
    public class SegmentServiceGetByNameTests
    {
        private readonly ICosmosRepository<HowToBecomeSegmentModel> repository;
        private readonly IHowToBecomeSegmentService howToBecomeSegmentService;

        public SegmentServiceGetByNameTests()
        {
            var jobProfileSegmentRefreshService = A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
            var mapper = A.Fake<AutoMapper.IMapper>();
            repository = A.Fake<ICosmosRepository<HowToBecomeSegmentModel>>();
            howToBecomeSegmentService = new HowToBecomeSegmentService(repository, jobProfileSegmentRefreshService, mapper);
        }

        [Fact]
        public async Task SegmentServiceGetByNameReturnsSuccess()
        {
            // arrange
            var expectedResult = A.Fake<HowToBecomeSegmentModel>();
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HowToBecomeSegmentModel, bool>>>.Ignored)).Returns(expectedResult);

            // act
            var result = await howToBecomeSegmentService.GetByNameAsync("article-name").ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HowToBecomeSegmentModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task SegmentServiceGetByNameReturnsArgumentNullExceptionWhenNullIsUsed()
        {
            // arrange

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await howToBecomeSegmentService.GetByNameAsync(null).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'canonicalName')", exceptionResult.Message);
        }

        [Fact]
        public async Task SegmentServiceGetByNameReturnsNullWhenMissingInRepository()
        {
            // arrange
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HowToBecomeSegmentModel, bool>>>.Ignored)).Returns((HowToBecomeSegmentModel)null);

            // act
            var result = await howToBecomeSegmentService.GetByNameAsync("article-name").ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HowToBecomeSegmentModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Null(result);
        }
    }
}