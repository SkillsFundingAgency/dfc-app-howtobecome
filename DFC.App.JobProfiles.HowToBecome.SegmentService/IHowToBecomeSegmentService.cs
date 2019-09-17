using DFC.App.JobProfiles.HowToBecome.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.SegmentService
{
    public interface IHowToBecomeSegmentService
    {
        Task<bool> PingAsync();

        Task<IEnumerable<HowToBecomeSegmentModel>> GetAllAsync();

        Task<HowToBecomeSegmentModel> GetByIdAsync(Guid documentId);

        Task<HowToBecomeSegmentModel> GetByNameAsync(string canonicalName, bool isDraft = false);

        Task<UpsertHowToBecomeSegmentModelResponse> UpsertAsync(HowToBecomeSegmentModel howToBecomeSegmentModel);

        Task<bool> DeleteAsync(Guid documentId);
    }
}