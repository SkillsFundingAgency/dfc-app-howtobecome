using DFC.Api.JobProfiles.Common.APISupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support
{
    internal partial class CommonAction : IAPISupport
    {
        public async Task<Response<T>> ExecuteGetRequest<T>(string endpoint, bool authoriseRequest = true)
        {
            GetRequest getRequest = new GetRequest(endpoint);
            getRequest.AddAcceptHeader(GetRequest.AcceptType.Json);
            getRequest.AddVersionHeader(Settings.APIConfig.Version);

            if (authoriseRequest)
            {
                getRequest.AddApimKeyHeader(Settings.APIConfig.ApimSubscriptionKey);
            }
            else
            {
                getRequest.AddApimKeyHeader(this.RandomString(20).ToLower(CultureInfo.CurrentCulture));
            }

            Response<T> response = getRequest.Execute<T>();
            DateTime startTime = DateTime.Now;
            while (response.HttpStatusCode.Equals(HttpStatusCode.NoContent) && DateTime.Now - startTime < Settings.GracePeriod)
            {
                await Task.Delay(500).ConfigureAwait(true);
                response = getRequest.Execute<T>();
            }

            return response;
        }
    }
}