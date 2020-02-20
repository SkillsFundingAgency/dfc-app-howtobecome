using DFC.App.JobProfiles.HowToBecome.Tests.Common.APISupport.Interface;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Threading;

namespace DFC.Api.JobProfiles.Common.APISupport
{
    public class GetRequest : IGetRequest
    {
        public GetRequest(string endpoint)
        {
            this.Request = new RestRequest(endpoint, Method.GET);
        }

        public enum AcceptType
        {
            Json,
            Html,
        }

        private RestRequest Request { get; set; }

        public void AddVersionHeader(string version)
        {
            this.Request.AddHeader("version", version);
        }

        public void AddApimKeyHeader(string apimSubscriptionKey)
        {
            this.Request.AddHeader("Ocp-Apim-Subscription-Key", apimSubscriptionKey);
        }

        public void AddAcceptHeader(AcceptType contentType)
        {
            switch (contentType)
            {
                case AcceptType.Json:
                    this.Request.AddHeader("Accept", "application/json");
                    break;

                case AcceptType.Html:
                    this.Request.AddHeader("Accept", "text/html");
                    break;
            }
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
    }
}
