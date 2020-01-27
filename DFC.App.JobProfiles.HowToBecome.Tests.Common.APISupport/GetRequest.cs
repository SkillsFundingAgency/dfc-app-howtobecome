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

        public GetRequest(string endpoint, ContentType contentType)
        {
            Request = new RestRequest(endpoint, Method.GET);
            AddContentType(contentType);
        }

        public void AddQueryParameter(string name, string value)
        {
            Request.AddParameter(name, value);
        }

        public void AddVersionHeader(string version)
        {
            Request.AddHeader("version", version);
            Headers.Add("version", version);
        }

        public void AddApimKeyHeader(string apimSubscriptionKey)
        {
            Request.AddHeader("Ocp-Apim-Subscription-Key", apimSubscriptionKey);
            Headers.Add("Ocp-Apim-Subscription-Key", apimSubscriptionKey);
        }

        public void AddContentType(ContentType contentType)
        {
            switch(contentType)
            {
                case ContentType.Json:
                    Request.AddHeader("Content-Type", "application/json");
                    Headers.Add("Content-Type", "application/json");
                    break;

                case ContentType.Html:
                    Request.AddHeader("Content-Type", "text/html");
                    Headers.Add("Content-Type", "text/html");
                    break;
            }
        }

        public enum ContentType
        {
            Json,
            Html
        }

        public Response<T> Execute<T>()
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            Response<T> response = new Response<T>();
            IRestResponse rawResponse = null;
            
            new RestClient().ExecuteAsync(Request, (IRestResponse apiResponse) => { 
                rawResponse = apiResponse;
                autoResetEvent.Set();
            });

            autoResetEvent.WaitOne();
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

            new RestClient().ExecuteAsync(Request, (IRestResponse apiResponse) => {
                rawResponse = apiResponse;
                autoResetEvent.Set();
            });

            autoResetEvent.WaitOne();
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
