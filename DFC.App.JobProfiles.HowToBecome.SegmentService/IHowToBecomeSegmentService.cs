using DFC.App.JobProfiles.HowToBecome.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.SegmentService
{
    public interface IHowToBecomeSegmentService
    {
        Task<IEnumerable<HowToBecomeSegmentModel>> GetAllAsync();

        Task<HowToBecomeSegmentModel> GetByIdAsync(Guid documentId);

        Task<HowToBecomeSegmentModel> GetByNameAsync(string canonicalName, bool isDraft = false);
    }
}