using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels;
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
            var url = new Uri($"{segmentClientOptions.BaseAddress}segment/{id}/contents");
            var response = await httpClient.GetAsync(url).ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return default;
            }

            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<HowToBecomeSegmentDataModel>(responseString);
        }

        public async Task<HttpStatusCode> PatchAsync<T>(T patchModel, string patchTypeEndpoint)
            where T : BasePatchModel
        {
            var url = new Uri($"{segmentClientOptions.BaseAddress}segment/{patchModel?.JobProfileId}/{patchTypeEndpoint}");
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

        public async Task<HttpStatusCode> PostFullJobProfileAsync(HowToBecomeSegmentModel howToBecomeSegmentModel)
        {
            var url = new Uri($"{segmentClientOptions?.BaseAddress}segment");

            using (var content = new ObjectContent(typeof(HowToBecomeSegmentModel), howToBecomeSegmentModel, new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json))
            {
                var response = await httpClient.PostAsync(url, content).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> PutFullJobProfileAsync(HowToBecomeSegmentModel howToBecomeSegmentModel)
        {
            var url = new Uri($"{segmentClientOptions?.BaseAddress}segment");

            using (var content = new ObjectContent(typeof(HowToBecomeSegmentModel), howToBecomeSegmentModel, new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json))
            {
                var response = await httpClient.PutAsync(url, content).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    logger.LogError($"Failure status code '{response.StatusCode}' received with content '{responseContent}', for Put type {typeof(HowToBecomeSegmentModel)}, Id: {howToBecomeSegmentModel?.DocumentId}");
                    response.EnsureSuccessStatusCode();
                }

                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> DeleteAsync(Guid id)
        {
            var url = new Uri($"{segmentClientOptions?.BaseAddress}segment/{id}");
            var response = await httpClient.DeleteAsync(url).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError($"Failure status code '{response.StatusCode}' received for Deleting Job Profile with Id: {id}");
                response.EnsureSuccessStatusCode();
            }

            return response.StatusCode;
        }
    }
}