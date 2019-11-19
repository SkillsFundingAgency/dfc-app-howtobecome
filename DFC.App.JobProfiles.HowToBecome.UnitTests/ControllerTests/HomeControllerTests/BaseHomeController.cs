﻿using DFC.App.JobProfiles.HowToBecome.Controllers;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Mime;

namespace DFC.App.JobProfiles.HowToBecome.UnitTests.ControllerTests.HomeControllerTests
{
    public class BaseHomeController
    {
        public BaseHomeController()
        {
            FakeLogger = A.Fake<ILogger<HomeController>>();
        }

        public static IEnumerable<object[]> HtmlMediaTypes => new List<object[]>
        {
            new object[] { "*/*" },
            new object[] { MediaTypeNames.Text.Html },
        };

        protected ILogger<HomeController> FakeLogger { get; }

        protected HomeController BuildHomeController(string mediaTypeName)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = mediaTypeName;

            return new HomeController(FakeLogger)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                },
            };
        }
    }
}