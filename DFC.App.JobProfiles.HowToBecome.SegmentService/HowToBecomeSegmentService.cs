using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.DraftSegmentService;
using DFC.App.JobProfiles.HowToBecome.Repository.CosmosDb;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.SegmentService
{
    public class HowToBecomeSegmentService : IHowToBecomeSegmentService
    {
        private readonly ICosmosRepository<HowToBecomeSegmentModel> repository;
        private readonly IDraftHowToBecomeSegmentService draftHowToBecomeSegmentService;

        public HowToBecomeSegmentService(ICosmosRepository<HowToBecomeSegmentModel> repository, IDraftHowToBecomeSegmentService draftHowToBecomeSegmentService)
        {
            this.repository = repository;
            this.draftHowToBecomeSegmentService = draftHowToBecomeSegmentService;
        }

        public async Task<IEnumerable<HowToBecomeSegmentModel>> GetAllAsync()
        {
            return await repository.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<HowToBecomeSegmentModel> GetByIdAsync(Guid documentId)
        {
            return await repository.GetAsync(d => d.DocumentId == documentId).ConfigureAwait(false);
        }

        public async Task<HowToBecomeSegmentModel> GetByNameAsync(string canonicalName, bool isDraft = false)
        {
            if (string.IsNullOrWhiteSpace(canonicalName))
            {
                throw new ArgumentNullException(nameof(canonicalName));
            }

            return isDraft
                ? await draftHowToBecomeSegmentService.GetSitefinityData(canonicalName.ToLowerInvariant()).ConfigureAwait(false)
                : await repository.GetAsync(d => d.CanonicalName == canonicalName.ToLowerInvariant()).ConfigureAwait(false);
        }
    }
}