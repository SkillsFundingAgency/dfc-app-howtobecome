﻿using AutoMapper;
using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.DataModels;
using DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.Enums;
using DFC.App.JobProfiles.HowToBecome.Repository.CosmosDb;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobProfiles.HowToBecome.SegmentService.UnitTests
{
    [Trait("Profile Service", "Patch Requirements Tests")]
    public class SegmentServicePatchRequirementsTests
    {
        private readonly IMapper mapper;
        private readonly ICosmosRepository<HowToBecomeSegmentModel> repository;
        private readonly IHowToBecomeSegmentService howToBecomeSegmentService;
        private readonly IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel> jobProfileSegmentRefreshService;

        public SegmentServicePatchRequirementsTests()
        {
            jobProfileSegmentRefreshService = A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
            mapper = A.Fake<IMapper>();
            repository = A.Fake<ICosmosRepository<HowToBecomeSegmentModel>>();
            howToBecomeSegmentService = new HowToBecomeSegmentService(repository, jobProfileSegmentRefreshService, mapper);
        }

        [Fact]
        public async Task HowToBecomeSegmentServicePatchRequirementsReturnsExceptionWhenPatchmodelIsNull()
        {
            // arrange
            PatchRequirementsModel patchModel = null;
            var documentId = Guid.NewGuid();

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await howToBecomeSegmentService.PatchRequirementsAsync(patchModel, documentId).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'patchModel')", exceptionResult.Message);
        }

        [Fact]
        public async Task HowToBecomeSegmentServicePatchRequirementsReturnsNotFoundWhenDocumentNotExists()
        {
            // arrange
            var patchModel = A.Fake<PatchRequirementsModel>();
            var documentId = Guid.NewGuid();
            HowToBecomeSegmentModel existingSegmentModel = null;
            var expectedResult = HttpStatusCode.NotFound;

            A.CallTo(() => repository.GetAsync(A<Expression<Func<HowToBecomeSegmentModel, bool>>>.Ignored)).Returns(existingSegmentModel);

            // act
            var result = await howToBecomeSegmentService.PatchRequirementsAsync(patchModel, documentId).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HowToBecomeSegmentModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task HowToBecomeSegmentServicePatchRequirementsReturnsAlreadyReportedWhenOutOfSequence()
        {
            // arrange
            var patchModel = A.Fake<PatchRequirementsModel>();
            var documentId = Guid.NewGuid();
            var existingSegmentModel = A.Fake<HowToBecomeSegmentModel>();
            var expectedResult = HttpStatusCode.AlreadyReported;

            patchModel.SequenceNumber = 1;
            patchModel.MessageAction = MessageAction.Published;
            existingSegmentModel.SequenceNumber = patchModel.SequenceNumber + 1;

            A.CallTo(() => repository.GetAsync(A<Expression<Func<HowToBecomeSegmentModel, bool>>>.Ignored)).Returns(existingSegmentModel);

            // act
            var result = await howToBecomeSegmentService.PatchRequirementsAsync(patchModel, documentId).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HowToBecomeSegmentModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task HowToBecomeSegmentServicePatchRequirementsReturnsAlreadyReportedWhenEntryRequirementForDeleted()
        {
            // arrange
            var patchModel = A.Fake<PatchRequirementsModel>();
            var documentId = Guid.NewGuid();
            var existingSegmentModel = A.Fake<HowToBecomeSegmentModel>();
            var expectedResult = HttpStatusCode.AlreadyReported;

            existingSegmentModel.SequenceNumber = 1;
            patchModel.SequenceNumber = existingSegmentModel.SequenceNumber + 1;
            patchModel.MessageAction = MessageAction.Deleted;

            A.CallTo(() => repository.GetAsync(A<Expression<Func<HowToBecomeSegmentModel, bool>>>.Ignored)).Returns(existingSegmentModel);

            // act
            var result = await howToBecomeSegmentService.PatchRequirementsAsync(patchModel, documentId).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HowToBecomeSegmentModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task HowToBecomeSegmentServicePatchRequirementsReturnsNotFoundWhenMissingEntryRequirementForPublished()
        {
            // arrange
            var patchModel = A.Fake<PatchRequirementsModel>();
            var documentId = Guid.NewGuid();
            var existingSegmentModel = A.Fake<HowToBecomeSegmentModel>();
            var expectedResult = HttpStatusCode.NotFound;

            existingSegmentModel.SequenceNumber = 1;
            patchModel.SequenceNumber = existingSegmentModel.SequenceNumber + 1;
            patchModel.MessageAction = MessageAction.Published;

            A.CallTo(() => repository.GetAsync(A<Expression<Func<HowToBecomeSegmentModel, bool>>>.Ignored)).Returns(existingSegmentModel);

            // act
            var result = await howToBecomeSegmentService.PatchRequirementsAsync(patchModel, documentId).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HowToBecomeSegmentModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task HowToBecomeSegmentServicePatchRequirementsReturnsOkWhenDeleted()
        {
            // arrange
            const RouteName routeName = RouteName.College;
            var patchModelId = Guid.NewGuid();
            var patchModel = A.Fake<PatchRequirementsModel>();
            var documentId = Guid.NewGuid();
            var existingSegmentModel = A.Fake<HowToBecomeSegmentModel>();
            var refreshJobProfileSegmentServiceBusModel = A.Fake<RefreshJobProfileSegmentServiceBusModel>();
            var expectedResult = HttpStatusCode.OK;

            existingSegmentModel.SequenceNumber = 1;
            existingSegmentModel.Data = A.Fake<HowToBecomeSegmentDataModel>();
            existingSegmentModel.Data.EntryRoutes = A.Fake<EntryRoutes>();
            existingSegmentModel.Data.EntryRoutes.CommonRoutes = new List<CommonRoutes>
            {
                new CommonRoutes
                {
                    RouteName = routeName,
                    EntryRequirements = new List<EntryRequirement>
                    {
                        new EntryRequirement
                        {
                            Id = patchModelId,
                        },
                    },
                },
            };

            patchModel.SequenceNumber = existingSegmentModel.SequenceNumber + 1;
            patchModel.MessageAction = MessageAction.Deleted;
            patchModel.RouteName = routeName;
            patchModel.Id = patchModelId;

            A.CallTo(() => repository.GetAsync(A<Expression<Func<HowToBecomeSegmentModel, bool>>>.Ignored)).Returns(existingSegmentModel);
            A.CallTo(() => mapper.Map<RefreshJobProfileSegmentServiceBusModel>(existingSegmentModel)).Returns(refreshJobProfileSegmentServiceBusModel);
            A.CallTo(() => repository.UpsertAsync(existingSegmentModel)).Returns(expectedResult);

            // act
            var result = await howToBecomeSegmentService.PatchRequirementsAsync(patchModel, documentId).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HowToBecomeSegmentModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => mapper.Map<RefreshJobProfileSegmentServiceBusModel>(existingSegmentModel)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.UpsertAsync(existingSegmentModel)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task HowToBecomeSegmentServicePatchRequirementsReturnsNotFoundWhenCreated()
        {
            // arrange
            var patchModel = A.Fake<PatchRequirementsModel>();
            var documentId = Guid.NewGuid();
            var existingSegmentModel = A.Fake<HowToBecomeSegmentModel>();
            var expectedResult = HttpStatusCode.NotFound;

            existingSegmentModel.SequenceNumber = 1;
            patchModel.SequenceNumber = existingSegmentModel.SequenceNumber + 1;
            patchModel.MessageAction = MessageAction.Published;

            A.CallTo(() => repository.GetAsync(A<Expression<Func<HowToBecomeSegmentModel, bool>>>.Ignored)).Returns(existingSegmentModel);

            // act
            var result = await howToBecomeSegmentService.PatchRequirementsAsync(patchModel, documentId).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HowToBecomeSegmentModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task HowToBecomeSegmentServicePatchRequirementsReturnsOkWhenUpdated()
        {
            // arrange
            const RouteName routeName = RouteName.College;
            var patchModelId = Guid.NewGuid();
            var patchModel = A.Fake<PatchRequirementsModel>();
            var documentId = Guid.NewGuid();
            var existingSegmentModel = A.Fake<HowToBecomeSegmentModel>();
            var refreshJobProfileSegmentServiceBusModel = A.Fake<RefreshJobProfileSegmentServiceBusModel>();
            var expectedResult = HttpStatusCode.OK;

            existingSegmentModel.SequenceNumber = 1;
            existingSegmentModel.Data = A.Fake<HowToBecomeSegmentDataModel>();
            existingSegmentModel.Data.EntryRoutes = A.Fake<EntryRoutes>();
            existingSegmentModel.Data.EntryRoutes.CommonRoutes = new List<CommonRoutes>
            {
                new CommonRoutes
                {
                    RouteName = routeName,
                    EntryRequirements = new List<EntryRequirement>
                    {
                        new EntryRequirement
                        {
                            Id = patchModelId,
                        },
                    },
                },
            };

            patchModel.SequenceNumber = existingSegmentModel.SequenceNumber + 1;
            patchModel.MessageAction = MessageAction.Published;
            patchModel.RouteName = routeName;
            patchModel.Id = patchModelId;

            A.CallTo(() => repository.GetAsync(A<Expression<Func<HowToBecomeSegmentModel, bool>>>.Ignored)).Returns(existingSegmentModel);
            A.CallTo(() => mapper.Map<RefreshJobProfileSegmentServiceBusModel>(existingSegmentModel)).Returns(refreshJobProfileSegmentServiceBusModel);
            A.CallTo(() => repository.UpsertAsync(existingSegmentModel)).Returns(expectedResult);

            // act
            var result = await howToBecomeSegmentService.PatchRequirementsAsync(patchModel, documentId).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HowToBecomeSegmentModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => mapper.Map<RefreshJobProfileSegmentServiceBusModel>(existingSegmentModel)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.UpsertAsync(existingSegmentModel)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }
    }
}
