using DFC.App.JobProfiles.HowToBecome.Data.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using Xunit;

namespace DFC.App.JobProfiles.HowToBecome.UnitTests.ControllerTests.SegmentControllerTests
{
    [Trait("Segment Controller", "Put Tests")]
    public class SegmentControllerPutTests : BaseSegmentController
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void SegmentControllerPutReturnsSuccessForUpdate(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var existingModel = A.Fake<HowToBecomeSegmentModel>();
            existingModel.SequenceNumber = 123;

            var modelToUpsert = A.Fake<HowToBecomeSegmentModel>();
            modelToUpsert.SequenceNumber = existingModel.SequenceNumber + 1;

            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeHowToBecomeSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns(existingModel);
            A.CallTo(() => FakeHowToBecomeSegmentService.UpsertAsync(A<HowToBecomeSegmentModel>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await controller.Put(modelToUpsert).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHowToBecomeSegmentService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeHowToBecomeSegmentService.UpsertAsync(A<HowToBecomeSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)expectedResponse, statusCodeResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void SegmentControllerPutReturnsAlreadyReportedForUpdate(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.AlreadyReported;
            var existingModel = A.Fake<HowToBecomeSegmentModel>();
            existingModel.SequenceNumber = 123;

            var modelToUpsert = A.Fake<HowToBecomeSegmentModel>();
            modelToUpsert.SequenceNumber = existingModel.SequenceNumber - 1;

            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeHowToBecomeSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns(existingModel);

            // Act
            var result = await controller.Put(modelToUpsert).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHowToBecomeSegmentService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)expectedResponse, statusCodeResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void SegmentControllerPutReturnsNotFoundForUpdate(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.NotFound;
            var modelToUpsert = A.Fake<HowToBecomeSegmentModel>();

            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeHowToBecomeSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns((HowToBecomeSegmentModel)null);

            // Act
            var result = await controller.Put(modelToUpsert).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHowToBecomeSegmentService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)expectedResponse, statusCodeResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void SegmentControllerPutReturnsBadResultWhenModelIsNull(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.BadRequest;
            var controller = BuildSegmentController(mediaTypeName);

            // Act
            var result = await controller.Put(null).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal((int)expectedResponse, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void SegmentControllerPutReturnsBadResultWhenModelIsInvalid(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.BadRequest;
            var howToBecomeSegmentModel = A.Fake<HowToBecomeSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);

            controller.ModelState.AddModelError(string.Empty, "Model is not valid");

            // Act
            var result = await controller.Put(howToBecomeSegmentModel).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)expectedResponse, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}