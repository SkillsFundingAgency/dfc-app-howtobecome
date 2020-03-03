using DFC.App.JobProfiles.HowToBecome.Data.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobProfiles.HowToBecome.UnitTests.ControllerTests.SegmentControllerTests
{
    [Trait("Segment Controller", "Post Tests")]
    public class SegmentControllerpostTests : BaseSegmentController
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task SegmentControllerPostReturnsSuccessForCreate(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.Created;
            var howToBecomeSegmentModel = A.Fake<HowToBecomeSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeHowToBecomeSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns((HowToBecomeSegmentModel)null);
            A.CallTo(() => FakeHowToBecomeSegmentService.UpsertAsync(A<HowToBecomeSegmentModel>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await controller.Post(howToBecomeSegmentModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHowToBecomeSegmentService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeHowToBecomeSegmentService.UpsertAsync(A<HowToBecomeSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)expectedResponse, statusCodeResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task SegmentControllerPostReturnsAlreadyReportedForCreate(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.AlreadyReported;
            var howToBecomeSegmentModel = A.Fake<HowToBecomeSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeHowToBecomeSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns(new HowToBecomeSegmentModel());

            // Act
            var result = await controller.Post(howToBecomeSegmentModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHowToBecomeSegmentService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)expectedResponse, statusCodeResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task SegmentControllerPostReturnsBadResultWhenModelIsNull(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.BadRequest;
            var controller = BuildSegmentController(mediaTypeName);

            // Act
            var result = await controller.Post(null).ConfigureAwait(false);

            // Assert
            var statusCodeResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal((int)expectedResponse, statusCodeResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task SegmentControllerPostReturnsBadResultWhenModelIsInvalid(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.BadRequest;
            var howToBecomeSegmentModel = A.Fake<HowToBecomeSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);

            controller.ModelState.AddModelError(string.Empty, "Model is not valid");

            // Act
            var result = await controller.Post(howToBecomeSegmentModel).ConfigureAwait(false);

            // Assert
            var statusCodeResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)expectedResponse, statusCodeResult.StatusCode);

            controller.Dispose();
        }
    }
}