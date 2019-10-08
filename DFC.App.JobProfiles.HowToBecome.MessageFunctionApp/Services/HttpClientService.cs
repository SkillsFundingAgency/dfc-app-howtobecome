using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels;
using DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Extensions;
using DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Models;
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

        public HttpClientService(SegmentClientOptions segmentClientOptions, HttpClient httpClient)
        {
            this.segmentClientOptions = segmentClientOptions;
            this.httpClient = httpClient;
        }

        public async Task<HowToBecomeSegmentDataModel> GetByIdAsync(Guid id)
        {
            if (httpClient == null)
            {
                return default;
            }

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
            if (httpClient == null)
            {
                return default;
            }

            var url = segmentClientOptions?.PatchLinksEndpoint.GetFormattedUrl(segmentClientOptions.BaseAddress, patchLinksModel?.JobProfileId);

            using (var request = new HttpRequestMessage(HttpMethod.Patch, url))
            {
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
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

            var url = segmentClientOptions?.DeleteEndpoint.GetFormattedUrl(segmentClientOptions.BaseAddress, id);

            using (var request = new HttpRequestMessage(HttpMethod.Delete, url))
            {
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

                var response = await httpClient.SendAsync(request).ConfigureAwait(false);

                return response.StatusCode;
            }
        }
    }
}