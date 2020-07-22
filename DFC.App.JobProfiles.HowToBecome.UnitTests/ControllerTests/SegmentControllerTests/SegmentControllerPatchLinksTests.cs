using DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.JobProfiles.HowToBecome.UnitTests.ControllerTests.SegmentControllerTests
{
    [Trait("Segment Controller", "Patch Links Tests")]
    public class SegmentControllerPatchLinksTests : BaseSegmentController
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task SegmentControllerPatchLinksReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var patchModel = A.Fake<PatchLinksModel>();
            var documentId = Guid.NewGuid();
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeHowToBecomeSegmentService.PatchLinksAsync(patchModel, documentId)).Returns(expectedResponse);

            // Act
            var result = await controller.PatchLinks(patchModel, documentId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHowToBecomeSegmentService.PatchLinksAsync(patchModel, documentId)).MustHaveHappenedOnceExactly();
            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)expectedResponse, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task SegmentControllerPatchLinksReturnsNotFound(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.NotFound;
            var patchModel = A.Fake<PatchLinksModel>();
            var documentId = Guid.NewGuid();
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeHowToBecomeSegmentService.PatchLinksAsync(patchModel, documentId)).Returns(expectedResponse);

            // Act
            var result = await controller.PatchLinks(patchModel, documentId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHowToBecomeSegmentService.PatchLinksAsync(patchModel, documentId)).MustHaveHappenedOnceExactly();
            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)expectedResponse, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task SegmentControllerPatchLinksReturnsBadRequestWhenNullPatchmodel(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.BadRequest;
            PatchLinksModel patchModel = null;
            var documentId = Guid.NewGuid();
            var controller = BuildSegmentController(mediaTypeName);

            // Act
            var result = await controller.PatchLinks(patchModel, documentId).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal((int)expectedResponse, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task SegmentControllerPatchLinksReturnsBadRequestWhenInvalidPatchmodel(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.BadRequest;
            var patchModel = A.Fake<PatchLinksModel>();
            var documentId = Guid.NewGuid();
            var controller = BuildSegmentController(mediaTypeName);

            controller.ModelState.AddModelError(string.Empty, "Model is not valid");

            // Act
            var result = await controller.PatchLinks(patchModel, documentId).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)expectedResponse, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}