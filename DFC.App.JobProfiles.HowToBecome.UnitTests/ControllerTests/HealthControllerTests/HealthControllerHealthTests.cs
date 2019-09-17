using DFC.App.JobProfiles.HowToBecome.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Net.Mime;
using Xunit;

namespace DFC.App.JobProfiles.HowToBecome.UnitTests.ControllerTests.HealthControllerTests
{
    [Trait("Health Controller", "Health Tests")]
    public class HealthControllerHealthTests : BaseHealthController
    {
        [Fact]
        public async void HealthControllerHealthReturnsSuccessWhenhealthy()
        {
            // Arrange
            var controller = BuildHealthController(MediaTypeNames.Application.Json);

            A.CallTo(() => FakeHowToBecomeSegmentService.PingAsync()).Returns(true);

            // Act
            var result = await controller.Health().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHowToBecomeSegmentService.PingAsync()).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<HealthViewModel>(jsonResult.Value);

            Assert.True(model.HealthItems.Count > 0);
            Assert.True(!string.IsNullOrWhiteSpace(model.HealthItems.First().Service));
            Assert.True(!string.IsNullOrWhiteSpace(model.HealthItems.First().Message));

            controller.Dispose();
        }

        [Fact]
        public async void HealthControllerHealthReturnsServiceUnavailableWhenUnhealthy()
        {
            // Arrange
            var controller = BuildHealthController(MediaTypeNames.Application.Json);

            A.CallTo(() => FakeHowToBecomeSegmentService.PingAsync()).Returns(false);

            // Act
            var result = await controller.Health().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHowToBecomeSegmentService.PingAsync()).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            Assert.Equal((int)HttpStatusCode.ServiceUnavailable, statusResult.StatusCode);

            controller.Dispose();
        }

        [Fact]
        public async void HealthControllerHealthReturnsServiceUnavailableWhenException()
        {
            // Arrange
            var controller = BuildHealthController(MediaTypeNames.Application.Json);

            A.CallTo(() => FakeHowToBecomeSegmentService.PingAsync()).Throws<Exception>();

            // Act
            var result = await controller.Health().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHowToBecomeSegmentService.PingAsync()).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            Assert.Equal((int)HttpStatusCode.ServiceUnavailable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}