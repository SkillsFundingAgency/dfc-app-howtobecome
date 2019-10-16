using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Services
{
    public interface IHttpClientService
    {
        Task<HowToBecomeSegmentDataModel> GetByIdAsync(Guid id);

        Task<HttpStatusCode> PostFullJobProfileAsync(HowToBecomeSegmentModel howToBecomeSegmentModel);

        Task<HttpStatusCode> PutFullJobProfileAsync(HowToBecomeSegmentModel howToBecomeSegmentModel);

        Task<HttpStatusCode> DeleteAsync(Guid id);

        Task<HttpStatusCode> PatchAsync<T>(T patchModel, string patchTypeEndpoint)
            where T : BasePatchModel;
    }
}