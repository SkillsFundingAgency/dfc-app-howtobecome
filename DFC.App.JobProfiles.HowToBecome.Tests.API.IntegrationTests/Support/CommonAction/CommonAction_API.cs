using DFC.Api.JobProfiles.Common.APISupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.Interface;
using HtmlAgilityPack;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support
{
    internal partial class CommonAction : IAPISupport
    {
        public async Task<Response<T>> ExecuteGetRequestWithJsonResponse<T>(string endpoint, bool AuthoriseRequest = true)
        {
            GetRequest getRequest = new GetRequest(endpoint, GetRequest.ContentType.Json);
            getRequest.AddVersionHeader(Settings.APIConfig.Version);

            if (AuthoriseRequest)
            {
                getRequest.AddApimKeyHeader(Settings.APIConfig.ApimSubscriptionKey);
            }
            else
            {
                getRequest.AddApimKeyHeader(RandomString(20).ToLower());
            }

            Response<T> response = getRequest.Execute<T>();
            DateTime startTime = DateTime.Now;
            while (response.HttpStatusCode.Equals(HttpStatusCode.NoContent) && DateTime.Now - startTime < Settings.GracePeriod)
            {
                await Task.Delay(500);
                response = getRequest.Execute<T>();
            }

            return response;
        }

        public async Task<Response<HtmlDocument>> ExecuteGetRequestWithHtmlResponse(string endpoint, bool AuthoriseRequest = true)
        {
            GetRequest getRequest = new GetRequest(endpoint, GetRequest.ContentType.Html);
            getRequest.AddVersionHeader(Settings.APIConfig.Version);

            if (AuthoriseRequest)
            {
                getRequest.AddApimKeyHeader(Settings.APIConfig.ApimSubscriptionKey);
            }
            else
            {
                getRequest.AddApimKeyHeader(RandomString(20).ToLower());
            }

            Response<HtmlDocument> response = getRequest.Execute();
            DateTime startTime = DateTime.Now;
            while (response.HttpStatusCode.Equals(HttpStatusCode.NoContent) && DateTime.Now - startTime < Settings.GracePeriod)
            {
                await Task.Delay(500);
                response = getRequest.Execute();
            }

            return response;
        }
    }
}