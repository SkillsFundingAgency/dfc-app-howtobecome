using DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobProfiles.HowToBecome.UnitTests.ControllerTests.SegmentControllerTests
{
    [Trait("Segment Controller", "Patch Requirements Tests")]
    public class SegmentControllerPatchRequirementsTests : BaseSegmentController
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task SegmentControllerPatchRequirementsReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var patchModel = A.Fake<PatchRequirementsModel>();
            var documentId = Guid.NewGuid();
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeHowToBecomeSegmentService.PatchRequirementsAsync(patchModel, documentId)).Returns(expectedResponse);

            // Act
            var result = await controller.PatchRequirements(patchModel, documentId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHowToBecomeSegmentService.PatchRequirementsAsync(patchModel, documentId)).MustHaveHappenedOnceExactly();
            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)expectedResponse, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task SegmentControllerPatchRequirementsReturnsNotFound(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.NotFound;
            var patchModel = A.Fake<PatchRequirementsModel>();
            var documentId = Guid.NewGuid();
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeHowToBecomeSegmentService.PatchRequirementsAsync(patchModel, documentId)).Returns(expectedResponse);

            // Act
            var result = await controller.PatchRequirements(patchModel, documentId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHowToBecomeSegmentService.PatchRequirementsAsync(patchModel, documentId)).MustHaveHappenedOnceExactly();
            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)expectedResponse, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task SegmentControllerPatchRequirementsReturnsBadRequestWhenNullPatchmodel(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.BadRequest;
            PatchRequirementsModel patchModel = null;
            var documentId = Guid.NewGuid();
            var controller = BuildSegmentController(mediaTypeName);

            // Act
            var result = await controller.PatchRequirements(patchModel, documentId).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal((int)expectedResponse, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task SegmentControllerPatchRequirementsReturnsBadRequestWhenInvalidPatchmodel(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.BadRequest;
            var patchModel = A.Fake<PatchRequirementsModel>();
            var documentId = Guid.NewGuid();
            var controller = BuildSegmentController(mediaTypeName);

            controller.ModelState.AddModelError(string.Empty, "Model is not valid");

            // Act
            var result = await controller.PatchRequirements(patchModel, documentId).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)expectedResponse, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}