using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.SegmentService
{
    public interface IHowToBecomeSegmentService
    {
        Task<bool> PingAsync();

        Task<IEnumerable<HowToBecomeSegmentModel>> GetAllAsync();

        Task<HowToBecomeSegmentModel> GetByIdAsync(Guid documentId);

        Task<HowToBecomeSegmentModel> GetByNameAsync(string canonicalName, bool isDraft = false);

        Task<HttpStatusCode> UpsertAsync(HowToBecomeSegmentModel howToBecomeSegmentModel);

        Task<bool> DeleteAsync(Guid documentId);

        Task<HttpStatusCode> PatchLinksAsync(PatchLinksModel patchModel, Guid documentId);

        Task<HttpStatusCode> PatchRequirementsAsync(PatchRequirementsModel patchModel, Guid documentId);

        Task<HttpStatusCode> PatchSimpleClassificationAsync(PatchSimpleClassificationModel patchModel, Guid documentId);

        Task<HttpStatusCode> PatchRegistrationAsync(PatchRegistrationModel patchModel, Guid documentId);
    }
}