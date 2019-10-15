using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels;
using DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Extensions;
using DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient httpClient;
        private readonly SegmentClientOptions segmentClientOptions;
        private readonly ILogger logger;

        public HttpClientService(SegmentClientOptions segmentClientOptions, HttpClient httpClient, ILogger logger)
        {
            this.segmentClientOptions = segmentClientOptions;
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<HowToBecomeSegmentDataModel> GetByIdAsync(Guid id)
        {
            var url = segmentClientOptions?.GetEndpoint.GetFormattedUrl(segmentClientOptions.BaseAddress, id);

            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

                var response = await httpClient.SendAsync(request).ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return default;
                }

                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<HowToBecomeSegmentDataModel>(responseString);
            }
        }

        public async Task<HttpStatusCode> PatchLinksAsync(PatchLinksModel patchLinksModel)
        {
            var url = $"{segmentClientOptions.BaseAddress}/segment/{patchLinksModel?.JobProfileId}/links";

            using (var request = new HttpRequestMessage(HttpMethod.Patch, url))
            {
                request.Headers.Accept.Clear(); //why do we need this?
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json)); //why do we need this?
                request.Content = new ObjectContent(typeof(PatchLinksModel), patchLinksModel, new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json);

                var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
                {
                    //log error with message from server
                    response.EnsureSuccessStatusCode();
                }

                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> PatchAsync<T>(T patchModel, string patchTypeEndpoint)
            where T : BasePatchModel
        {
            var url = new Uri($"{segmentClientOptions.BaseAddress}/segment/{patchModel?.JobProfileId}/{patchTypeEndpoint}");
            using (var content = new ObjectContent<T>(patchModel, new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json))
            {
                var response = await httpClient.PatchAsync(url, content).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    logger.LogError($"Failure status code '{response.StatusCode}' received with content '{responseContent}', for patch type {typeof(T)}, Id: {patchModel.JobProfileId}");

                    response.EnsureSuccessStatusCode();
                }

                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> PatchRequirementsAsync(PatchRequirementsModel patchRequirementsModel)
        {
            if (httpClient == null)
            {
                return default;
            }

            var url = segmentClientOptions?.PatchRequirementsEndpoint.GetFormattedUrl(segmentClientOptions.BaseAddress, patchRequirementsModel.JobProfileId);

            using (var request = new HttpRequestMessage(HttpMethod.Patch, url))
            {
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
                request.Content = new ObjectContent(typeof(PatchRequirementsModel), patchRequirementsModel, new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json);

                var response = await httpClient.SendAsync(request).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
                {
                    //log error with message from server
                    response.EnsureSuccessStatusCode();
                }

                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> PatchSimpleClassificationAsync(PatchSimpleClassificationModel patchSimpleClassificationModel)
        {
            if (httpClient == null)
            {
                return default;
            }

            var url = segmentClientOptions?.PatchSimpleClassificationEndpoint.GetFormattedUrl(segmentClientOptions.BaseAddress, patchSimpleClassificationModel.JobProfileId);

            using (var request = new HttpRequestMessage(HttpMethod.Patch, url))
            {
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
                request.Content = new ObjectContent(typeof(PatchSimpleClassificationModel), patchSimpleClassificationModel, new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json);

                var response = await httpClient.SendAsync(request).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
                {
                    //log error with message from server
                    response.EnsureSuccessStatusCode();
                }

                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> PostFullJobProfileAsync(HowToBecomeSegmentModel howToBecomeSegmentModel)
        {
            if (httpClient == null)
            {
                return default;
            }

            var url = segmentClientOptions?.PostEndpoint.GetFormattedUrl(segmentClientOptions.BaseAddress);

            using (var request = new HttpRequestMessage(HttpMethod.Put, url))
            {
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
                request.Content = new ObjectContent(typeof(HowToBecomeSegmentModel), howToBecomeSegmentModel, new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json);

                var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> PutFullJobProfileAsync(HowToBecomeSegmentModel howToBecomeSegmentModel)
        {
            if (httpClient == null)
            {
                return default;
            }

            var url = segmentClientOptions?.PutEndpoint.GetFormattedUrl(segmentClientOptions.BaseAddress);

            using (var request = new HttpRequestMessage(HttpMethod.Patch, url))
            {
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
                request.Content = new ObjectContent(typeof(PatchSegmentModel), howToBecomeSegmentModel, new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json);

                var response = await httpClient.SendAsync(request).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
                {
                    //log error with message from server
                    response.EnsureSuccessStatusCode();
                }

                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> DeleteAsync(Guid id)
        {
            if (httpClient == null)
            {
                return default;
            }

            var url = new Uri(segmentClientOptions?.DeleteEndpoint.GetFormattedUrl(segmentClientOptions.BaseAddress, id));

            var response = await httpClient.DeleteAsync(url).ConfigureAwait(false);

            //Add Logging

            return response.StatusCode;
        }
    }
}