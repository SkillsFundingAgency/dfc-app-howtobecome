using DFC.App.JobProfiles.HowToBecome.Controllers;
using DFC.App.JobProfiles.HowToBecome.SegmentService;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace DFC.App.JobProfiles.HowToBecome.UnitTests.ControllerTests.HealthControllerTests
{
    public class BaseHealthController
    {
        public BaseHealthController()
        {
            FakeLogger = A.Fake<ILogger<HealthController>>();
            FakeHowToBecomeSegmentService = A.Fake<IHowToBecomeSegmentService>();
        }

        protected IHowToBecomeSegmentService FakeHowToBecomeSegmentService { get; }

        protected ILogger<HealthController> FakeLogger { get; }

        protected HealthController BuildHealthController(string mediaTypeName)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = mediaTypeName;

            return new HealthController(FakeLogger, FakeHowToBecomeSegmentService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                },
            };
        }
    }
}