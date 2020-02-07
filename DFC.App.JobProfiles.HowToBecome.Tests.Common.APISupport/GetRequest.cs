using HtmlAgilityPack;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace DFC.Api.JobProfiles.Common.APISupport
{
    public class GetRequest
    {
        private RestRequest Request { get; set; }
        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        public GetRequest(string endpoint)
        {
            this.Request = new RestRequest(endpoint, Method.GET);
        }

        public void AddQueryParameter(string name, string value)
        {
            this.Request.AddParameter(name, value);
        }

        public void AddVersionHeader(string version)
        {
            this.Request.AddHeader("version", version);
            this.Headers.Add("version", version);
        }

        public void AddApimKeyHeader(string apimSubscriptionKey)
        {
            this.Request.AddHeader("Ocp-Apim-Subscription-Key", apimSubscriptionKey);
            this.Headers.Add("Ocp-Apim-Subscription-Key", apimSubscriptionKey);
        }

        public void AddContentTypeHeader(ContentType contentType)
        {
            switch (contentType)
            {
                case ContentType.Json:
                    this.Request.AddHeader("Content-Type", "application/json");
                    this.Headers.Add("Content-Type", "application/json");
                    break;

                case ContentType.Html:
                    this.Request.AddHeader("Content-Type", "text/html");
                    this.Headers.Add("Content-Type", "text/html");
                    break;

                default:
                    throw new System.Exception("Unrecognised content type");
            }
        }

        public void AddAcceptHeader(AcceptType contentType)
        {
            switch (contentType)
            {
                case AcceptType.Json:
                    this.Request.AddHeader("Accept", "application/json");
                    this.Headers.Add("Accept", "application/json");
                    break;

                case AcceptType.Html:
                    this.Request.AddHeader("Accept", "text/html");
                    this.Headers.Add("Accept", "text/html");
                    break;
            }
        }

        public enum ContentType
        {
            Json,
            Html,
        }

        public enum AcceptType
        {
            Json,
            Html,
        }

        public Response<T> Execute<T>()
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            Response<T> response = new Response<T>();
            IRestResponse rawResponse = null;

            new RestClient().ExecuteAsync(this.Request, (IRestResponse apiResponse) =>
            {
                rawResponse = apiResponse;
                autoResetEvent.Set();
            });

            autoResetEvent.WaitOne();
            autoResetEvent.Dispose();
            response.HttpStatusCode = rawResponse.StatusCode;
            response.IsSuccessful = rawResponse.IsSuccessful;
            response.ErrorMessage = rawResponse.ErrorMessage;
            response.ResponseStatus = rawResponse.ResponseStatus;
            if (response.HttpStatusCode.Equals(HttpStatusCode.OK))
            {
                response.Data = JsonConvert.DeserializeObject<T>(rawResponse.Content);
            }

            return response;
        }

        public Response<HtmlDocument> Execute()
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            Response<HtmlDocument> response = new Response<HtmlDocument>();
            IRestResponse rawResponse = null;

            new RestClient().ExecuteAsync(this.Request, (IRestResponse apiResponse) =>
            {
                rawResponse = apiResponse;
                autoResetEvent.Set();
            });

            autoResetEvent.WaitOne();
            autoResetEvent.Dispose();
            response.HttpStatusCode = rawResponse.StatusCode;
            response.IsSuccessful = rawResponse.IsSuccessful;
            response.ErrorMessage = rawResponse.ErrorMessage;
            response.ResponseStatus = rawResponse.ResponseStatus;
            if (response.HttpStatusCode.Equals(HttpStatusCode.OK))
            {
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(rawResponse.Content);
                response.Data = htmlDocument;
            }

            return response;
        }
    }
}
