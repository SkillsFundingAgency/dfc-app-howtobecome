using DFC.App.CareerPath.Common.Contracts;
using DFC.App.JobProfiles.HowToBecome.Controllers;
using DFC.App.JobProfiles.HowToBecome.SegmentService;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Mime;

namespace DFC.App.JobProfiles.HowToBecome.UnitTests.ControllerTests.SegmentControllerTests
{
    public class BaseSegmentController
    {
        public BaseSegmentController()
        {
            FakeLogger = A.Fake<ILogService>();
            FakeHowToBecomeSegmentService = A.Fake<IHowToBecomeSegmentService>();
            FakeMapper = A.Fake<AutoMapper.IMapper>();
        }

        public static IEnumerable<object[]> HtmlMediaTypes => new List<object[]>
        {
            new object[] { "*/*" },
            new object[] { MediaTypeNames.Text.Html },
        };

        public static IEnumerable<object[]> InvalidMediaTypes => new List<object[]>
        {
            new object[] { MediaTypeNames.Text.Plain },
        };

        public static IEnumerable<object[]> JsonMediaTypes => new List<object[]>
        {
            new object[] { MediaTypeNames.Application.Json },
        };

        protected ILogService FakeLogger { get; }

        protected IHowToBecomeSegmentService FakeHowToBecomeSegmentService { get; }

        protected AutoMapper.IMapper FakeMapper { get; }

        protected SegmentController BuildSegmentController(string mediaTypeName)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = mediaTypeName;

            var controller = new SegmentController(FakeLogger, FakeHowToBecomeSegmentService, FakeMapper)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                },
            };

            return controller;
        }
    }
}