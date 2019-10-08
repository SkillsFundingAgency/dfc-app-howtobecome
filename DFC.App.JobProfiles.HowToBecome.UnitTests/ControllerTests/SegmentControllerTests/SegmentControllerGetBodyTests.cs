﻿using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using Xunit;

namespace DFC.App.JobProfiles.HowToBecome.UnitTests.ControllerTests.SegmentControllerTests
{
    [Trait("Segment Controller", "Get Body Tests")]
    public class SegmentControllerGetBodyTests : BaseSegmentController
    {
        private readonly Guid documentId = Guid.NewGuid();

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async void SegmentControllerGetBodyHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var expectedResult = A.Fake<HowToBecomeSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeHowToBecomeSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<HowToBecomeSegmentModel>.Ignored)).Returns(A.Fake<DocumentViewModel>());

            // Act
            var result = await controller.Body(documentId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHowToBecomeSegmentService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<HowToBecomeSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<DocumentViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async void SegmentControllerGetBodyHtmlReturnsNoContentWhenNoData(string mediaTypeName)
        {
            // Arrange
            var controller = BuildSegmentController(mediaTypeName);
            A.CallTo(() => FakeHowToBecomeSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns((HowToBecomeSegmentModel)null);

            // Act
            var result = await controller.Body(documentId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHowToBecomeSegmentService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            var statusResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal((int)HttpStatusCode.NoContent, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void SegmentControllerGetBodyJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var howToBecomeSegmentModel = new HowToBecomeSegmentModel { CanonicalName = "SomeCanonicalName" };
            var fakeDocumentViewModel = new DocumentViewModel { CanonicalName = howToBecomeSegmentModel.CanonicalName };

            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeHowToBecomeSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns(howToBecomeSegmentModel);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<HowToBecomeSegmentModel>.Ignored)).Returns(fakeDocumentViewModel);

            // Act
            var result = await controller.Body(documentId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHowToBecomeSegmentService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<HowToBecomeSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<DocumentViewModel>(jsonResult.Value);

            Assert.True(!string.IsNullOrWhiteSpace(model.CanonicalName) && model.CanonicalName.Equals(howToBecomeSegmentModel.CanonicalName, StringComparison.OrdinalIgnoreCase));

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async void SegmentControllerGetBodyPlainMediaTypeReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var expectedResult = A.Fake<HowToBecomeSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeHowToBecomeSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<HowToBecomeSegmentModel>.Ignored)).Returns(A.Fake<DocumentViewModel>());

            // Act
            var result = await controller.Body(documentId).ConfigureAwait(false);
            var viewResult = Assert.IsType<StatusCodeResult>(result);

            // Assert
            A.CallTo(() => FakeHowToBecomeSegmentService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<HowToBecomeSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal((int)HttpStatusCode.NotAcceptable, viewResult.StatusCode);
            controller.Dispose();
        }
    }
}