using System;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.JobProfiles.HowToBecome.UnitTests.ControllerTests.SegmentControllerTests
{
    [Trait("Segment Controller", "Create or Update Tests")]
    public class SegmentControllerUpsertTests : BaseSegmentController
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void SegmentControllerUpsertReturnsSuccessForCreate(string mediaTypeName)
        {
            // Arrange
            var howToBecomeSegmentModel = A.Fake<HowToBecomeSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);
            var expectedUpsertResponse = HttpStatusCode.Created;

            A.CallTo(() => FakeHowToBecomeSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns((HowToBecomeSegmentModel)null);
            A.CallTo(() => FakeHowToBecomeSegmentService.UpsertAsync(A<HowToBecomeSegmentModel>.Ignored)).Returns(expectedUpsertResponse);

            // Act
            var result = await controller.Post(howToBecomeSegmentModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHowToBecomeSegmentService.UpsertAsync(A<HowToBecomeSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();
            var okResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.Created, okResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void SegmentControllerUpsertReturnsSuccessForUpdate(string mediaTypeName)
        {
            // Arrange
            var existingModel = A.Fake<HowToBecomeSegmentModel>();
            existingModel.SequenceNumber = 123;

            var modelToUpsert = A.Fake<HowToBecomeSegmentModel>();
            modelToUpsert.SequenceNumber = 124;

            var controller = BuildSegmentController(mediaTypeName);
            var expectedUpsertResponse = HttpStatusCode.OK;

            A.CallTo(() => FakeHowToBecomeSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns(existingModel);
            A.CallTo(() => FakeHowToBecomeSegmentService.UpsertAsync(A<HowToBecomeSegmentModel>.Ignored)).Returns(expectedUpsertResponse);

            // Act
            var result = await controller.Put(modelToUpsert).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHowToBecomeSegmentService.UpsertAsync(A<HowToBecomeSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();
            var okResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void SegmentControllerUpsertReturnsBadResultWhenModelIsNull(string mediaTypeName)
        {
            // Arrange
            var controller = BuildSegmentController(mediaTypeName);

            // Act
            var result = await controller.Put(null).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void SegmentControllerUpsertReturnsBadResultWhenModelIsInvalid(string mediaTypeName)
        {
            // Arrange
            var howToBecomeSegmentModel = new HowToBecomeSegmentModel();
            var controller = BuildSegmentController(mediaTypeName);

            controller.ModelState.AddModelError(string.Empty, "Model is not valid");

            // Act
            var result = await controller.Put(howToBecomeSegmentModel).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}