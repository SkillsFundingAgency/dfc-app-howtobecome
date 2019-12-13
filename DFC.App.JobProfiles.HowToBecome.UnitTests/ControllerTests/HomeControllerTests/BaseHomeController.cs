using DFC.App.JobProfiles.HowToBecome.Controllers;
using DFC.Logger.AppInsights.Contracts;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Mime;

namespace DFC.App.JobProfiles.HowToBecome.UnitTests.ControllerTests.HomeControllerTests
{
    public class BaseHomeController
    {
        public BaseHomeController()
        {
            FakeLogger = A.Fake<ILogService>();
        }

        public static IEnumerable<object[]> HtmlMediaTypes => new List<object[]>
        {
            new object[] { "*/*" },
            new object[] { MediaTypeNames.Text.Html },
        };

        protected ILogService FakeLogger { get; }

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