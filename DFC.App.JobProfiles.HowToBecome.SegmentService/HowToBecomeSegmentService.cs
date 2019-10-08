using AutoMapper;
using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.DataModels;
using DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.Enums;
using DFC.App.JobProfiles.HowToBecome.DraftSegmentService;
using DFC.App.JobProfiles.HowToBecome.Repository.CosmosDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.SegmentService
{
    public class HowToBecomeSegmentService : IHowToBecomeSegmentService
    {
        private readonly ICosmosRepository<HowToBecomeSegmentModel> repository;
        private readonly IDraftHowToBecomeSegmentService draftHowToBecomeSegmentService;
        private readonly IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel> jobProfileSegmentRefreshService;
        private readonly IMapper mapper;

        public HowToBecomeSegmentService(ICosmosRepository<HowToBecomeSegmentModel> repository, IDraftHowToBecomeSegmentService draftHowToBecomeSegmentService, IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel> jobProfileSegmentRefreshService, IMapper mapper)
        {
            this.repository = repository;
            this.draftHowToBecomeSegmentService = draftHowToBecomeSegmentService;
            this.jobProfileSegmentRefreshService = jobProfileSegmentRefreshService;
            this.mapper = mapper;
        }

        public async Task<bool> PingAsync()
        {
            return await repository.PingAsync().ConfigureAwait(false);
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

        public async Task<HttpStatusCode> UpsertAsync(HowToBecomeSegmentModel howToBecomeSegmentModel)
        {
            if (howToBecomeSegmentModel == null)
            {
                throw new ArgumentNullException(nameof(howToBecomeSegmentModel));
            }

            if (howToBecomeSegmentModel.Data == null)
            {
                howToBecomeSegmentModel.Data = new HowToBecomeSegmentDataModel();
            }

            return await UpsertAndRefreshSegmentModel(howToBecomeSegmentModel).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(Guid documentId)
        {
            var result = await repository.DeleteAsync(documentId).ConfigureAwait(false);

            return result == HttpStatusCode.NoContent;
        }

        public async Task<HttpStatusCode> PatchLinksAsync(PatchLinksModel patchModel, Guid documentId)
        {
            if (patchModel == null)
            {
                throw new ArgumentNullException(nameof(patchModel));
            }

            var existingSegmentModel = await GetByIdAsync(documentId).ConfigureAwait(false);
            if (existingSegmentModel == null)
            {
                return HttpStatusCode.NotFound;
            }

            if (patchModel.SequenceNumber <= existingSegmentModel.SequenceNumber)
            {
                return HttpStatusCode.AlreadyReported;
            }

            var existingCommonRoute = GetExistingCommonRoute(existingSegmentModel, patchModel.RouteName);
            var linkToUpdate = existingCommonRoute?.AdditionalInformation.FirstOrDefault(a => a.Id == patchModel.Id);

            var updatedAdditionalInfo = mapper.Map<AdditionalInformation>(patchModel);

            var filteredAdditionalInfo = existingSegmentModel.Data.EntryRoutes.CommonRoutes
                .First(e => e.RouteName == patchModel.RouteName).AdditionalInformation
                .Where(ai => ai.Id != patchModel.Id).ToList();

            if (linkToUpdate != null)
            {
                if (patchModel.EventType == EventType.Published)
                {
                    var existingIndex = existingCommonRoute.AdditionalInformation.ToList().FindIndex(ai => ai.Id == patchModel.Id);
                    filteredAdditionalInfo.Insert(existingIndex, updatedAdditionalInfo);
                }
            }
            else
            {
                filteredAdditionalInfo.Append(updatedAdditionalInfo);
            }

            existingSegmentModel.SequenceNumber = patchModel.SequenceNumber;
            existingSegmentModel.Data.EntryRoutes.CommonRoutes.First(e => e.RouteName == patchModel.RouteName).AdditionalInformation = filteredAdditionalInfo;

            return await UpsertAndRefreshSegmentModel(existingSegmentModel).ConfigureAwait(false);
        }

        public async Task<HttpStatusCode> PatchRequirementsAsync(PatchRequirementsModel patchModel, Guid documentId)
        {
            if (patchModel == null)
            {
                throw new ArgumentNullException(nameof(patchModel));
            }

            var existingSegmentModel = await GetByIdAsync(documentId).ConfigureAwait(false);

            if (existingSegmentModel == null)
            {
                return HttpStatusCode.NotFound;
            }

            if (patchModel.SequenceNumber <= existingSegmentModel.SequenceNumber)
            {
                return HttpStatusCode.AlreadyReported;
            }

            var existingCommonRoute = GetExistingCommonRoute(existingSegmentModel, patchModel.RouteName);
            var existingRequirement = existingCommonRoute?.EntryRequirements.FirstOrDefault(r => r.Id == patchModel.Id);

            var updatedEntryRequirements = mapper.Map<EntryRequirement>(patchModel);

            var filteredEntryRequirements = existingSegmentModel.Data.EntryRoutes.CommonRoutes
                .First(e => e.RouteName == patchModel.RouteName).EntryRequirements
                .Where(ai => ai.Id != patchModel.Id).ToList();

            if (existingRequirement != null)
            {
                if (patchModel.EventType == EventType.Published)
                {
                    var existingIndex = existingCommonRoute.EntryRequirements.ToList().FindIndex(ai => ai.Id == patchModel.Id);
                    filteredEntryRequirements.Insert(existingIndex, updatedEntryRequirements);
                }
            }
            else
            {
                filteredEntryRequirements.Append(updatedEntryRequirements);
            }

            existingSegmentModel.SequenceNumber = patchModel.SequenceNumber;
            existingSegmentModel.Data.EntryRoutes.CommonRoutes.First(e => e.RouteName == patchModel.RouteName).EntryRequirements = filteredEntryRequirements;

            return await UpsertAndRefreshSegmentModel(existingSegmentModel).ConfigureAwait(false);
        }

        public async Task<HttpStatusCode> PatchSimpleClassificationAsync(PatchSimpleClassificationModel patchModel, Guid documentId)
        {
            if (patchModel == null)
            {
                throw new ArgumentNullException(nameof(patchModel));
            }

            var existingSegmentModel = await GetByIdAsync(documentId).ConfigureAwait(false);

            if (existingSegmentModel == null)
            {
                return HttpStatusCode.NotFound;
            }

            if (patchModel.SequenceNumber <= existingSegmentModel.SequenceNumber)
            {
                return HttpStatusCode.AlreadyReported;
            }

            var existingCommonRoute = GetExistingCommonRoute(existingSegmentModel, patchModel.RouteName);

            existingSegmentModel.SequenceNumber = patchModel.SequenceNumber;
            existingCommonRoute.EntryRequirementPreface =
                !string.IsNullOrEmpty(existingCommonRoute.EntryRequirementPreface) && patchModel.EventType == EventType.Published
                    ? string.Empty
                    : patchModel.Title;

            return await UpsertAndRefreshSegmentModel(existingSegmentModel).ConfigureAwait(false);
        }

        private static CommonRoutes GetExistingCommonRoute(HowToBecomeSegmentModel existingSegmentModel, RouteName routeName)
        {
            var existingCommonRoutes = existingSegmentModel.Data.EntryRoutes.CommonRoutes;
            return existingCommonRoutes.FirstOrDefault(r => r.RouteName == routeName);
        }

        private async Task<HttpStatusCode> UpsertAndRefreshSegmentModel(HowToBecomeSegmentModel existingSegmentModel)
        {
            var result = await repository.UpsertAsync(existingSegmentModel).ConfigureAwait(false);

            if (result == HttpStatusCode.OK || result == HttpStatusCode.Created)
            {
                var refreshJobProfileSegmentServiceBusModel = mapper.Map<RefreshJobProfileSegmentServiceBusModel>(existingSegmentModel);

                await jobProfileSegmentRefreshService.SendMessageAsync(refreshJobProfileSegmentServiceBusModel).ConfigureAwait(false);
            }

            return result;
        }
    }
}