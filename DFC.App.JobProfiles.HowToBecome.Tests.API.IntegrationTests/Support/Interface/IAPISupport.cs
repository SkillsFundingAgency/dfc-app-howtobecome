using DFC.Api.JobProfiles.Common.APISupport;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.Interface
{
    interface IAPISupport
    {
        Task<Response<T>> ExecuteGetRequestWithJsonResponse<T>(string endpoint, bool AuthoriseRequest = true);
        Task<Response<HtmlDocument>> ExecuteGetRequestWithHtmlResponse(string endpoint, bool AuthoriseRequest = true);
    }
}
