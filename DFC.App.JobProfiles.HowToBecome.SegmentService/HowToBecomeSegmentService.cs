using AutoMapper;
using DFC.App.JobProfiles.HowToBecome.Data;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.DataModels;
using DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.Enums;
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
        private readonly IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel> jobProfileSegmentRefreshService;
        private readonly IMapper mapper;

        public HowToBecomeSegmentService(ICosmosRepository<HowToBecomeSegmentModel> repository, IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel> jobProfileSegmentRefreshService, IMapper mapper)
        {
            this.repository = repository;
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

        public async Task<HowToBecomeSegmentModel> GetByNameAsync(string canonicalName)
        {
            if (string.IsNullOrWhiteSpace(canonicalName))
            {
                throw new ArgumentNullException(nameof(canonicalName));
            }

            return await repository.GetAsync(d => d.CanonicalName == canonicalName.ToLowerInvariant()).ConfigureAwait(false);
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
            if (patchModel is null)
            {
                throw new ArgumentNullException(nameof(patchModel));
            }

            var existingSegmentModel = await GetByIdAsync(documentId).ConfigureAwait(false);
            if (existingSegmentModel is null)
            {
                return HttpStatusCode.NotFound;
            }

            if (patchModel.SequenceNumber <= existingSegmentModel.SequenceNumber)
            {
                return HttpStatusCode.AlreadyReported;
            }

            var existingCommonRoute = existingSegmentModel.GetExistingCommonRoute(patchModel.RouteName);
            var linkToUpdate = existingCommonRoute?.AdditionalInformation?.FirstOrDefault(ai => ai.Id == patchModel.Id);

            if (linkToUpdate is null)
            {
                return patchModel.MessageAction == MessageAction.Deleted ? HttpStatusCode.AlreadyReported : HttpStatusCode.NotFound;
            }

            var existingIndex = existingCommonRoute.AdditionalInformation.ToList().FindIndex(ai => ai.Id == patchModel.Id);

            if (patchModel.MessageAction == MessageAction.Deleted)
            {
                existingSegmentModel.Data.EntryRoutes.CommonRoutes.First(e => e.RouteName == patchModel.RouteName).AdditionalInformation.RemoveAt(existingIndex);
            }
            else
            {
                var updatedAdditionalInfo = mapper.Map<AdditionalInformation>(patchModel);
                existingSegmentModel.Data.EntryRoutes.CommonRoutes.First(e => e.RouteName == patchModel.RouteName).AdditionalInformation[existingIndex] = updatedAdditionalInfo;
            }

            existingSegmentModel.SequenceNumber = patchModel.SequenceNumber;

            return await UpsertAndRefreshSegmentModel(existingSegmentModel).ConfigureAwait(false);
        }

        public async Task<HttpStatusCode> PatchRequirementsAsync(PatchRequirementsModel patchModel, Guid documentId)
        {
            if (patchModel is null)
            {
                throw new ArgumentNullException(nameof(patchModel));
            }

            var existingSegmentModel = await GetByIdAsync(documentId).ConfigureAwait(false);
            if (existingSegmentModel is null)
            {
                return HttpStatusCode.NotFound;
            }

            if (patchModel.SequenceNumber <= existingSegmentModel.SequenceNumber)
            {
                return HttpStatusCode.AlreadyReported;
            }

            var existingCommonRoute = existingSegmentModel.GetExistingCommonRoute(patchModel.RouteName);
            var existingRequirement = existingCommonRoute?.EntryRequirements.FirstOrDefault(r => r.Id == patchModel.Id);

            if (existingRequirement is null)
            {
                return patchModel.MessageAction == MessageAction.Deleted ? HttpStatusCode.AlreadyReported : HttpStatusCode.NotFound;
            }

            var existingIndex = existingCommonRoute.EntryRequirements.ToList().FindIndex(ai => ai.Id == patchModel.Id);

            if (patchModel.MessageAction == MessageAction.Deleted)
            {
                existingSegmentModel.Data.EntryRoutes.CommonRoutes.First(e => e.RouteName == patchModel.RouteName).EntryRequirements.RemoveAt(existingIndex);
            }
            else
            {
                var updatedEntryRequirements = mapper.Map<EntryRequirement>(patchModel);
                existingSegmentModel.Data.EntryRoutes.CommonRoutes.First(e => e.RouteName == patchModel.RouteName).EntryRequirements[existingIndex] = updatedEntryRequirements;
            }

            existingSegmentModel.SequenceNumber = patchModel.SequenceNumber;

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

            var existingCommonRoute = existingSegmentModel.GetExistingCommonRoute(patchModel.RouteName);
            if (existingCommonRoute is null)
            {
                return patchModel.MessageAction == MessageAction.Deleted ? HttpStatusCode.AlreadyReported : HttpStatusCode.NotFound;
            }

            existingCommonRoute.EntryRequirementPreface = patchModel.MessageAction == MessageAction.Deleted ? string.Empty : patchModel.Title;

            existingSegmentModel.SequenceNumber = patchModel.SequenceNumber;
            return await UpsertAndRefreshSegmentModel(existingSegmentModel).ConfigureAwait(false);
        }

        public async Task<HttpStatusCode> PatchRegistrationAsync(PatchRegistrationModel patchModel, Guid documentId)
        {
            if (patchModel is null)
            {
                throw new ArgumentNullException(nameof(patchModel));
            }

            var existingSegmentModel = await GetByIdAsync(documentId).ConfigureAwait(false);
            if (existingSegmentModel is null)
            {
                return HttpStatusCode.NotFound;
            }

            if (patchModel.SequenceNumber <= existingSegmentModel.SequenceNumber)
            {
                return HttpStatusCode.AlreadyReported;
            }

            var existingRegistration = existingSegmentModel.Data?.Registrations.FirstOrDefault(r => r.Id == patchModel.Id);
            if (existingRegistration is null)
            {
                return patchModel.MessageAction == MessageAction.Deleted ? HttpStatusCode.AlreadyReported : HttpStatusCode.NotFound;
            }

            var existingIndex = existingSegmentModel.Data.Registrations.ToList().FindIndex(ai => ai.Id == patchModel.Id);

            if (patchModel.MessageAction == MessageAction.Deleted)
            {
                existingSegmentModel.Data.Registrations.RemoveAt(existingIndex);
            }
            else
            {
                var updatedRegistration = mapper.Map<Registration>(patchModel);
                existingSegmentModel.Data.Registrations[existingIndex] = updatedRegistration;
            }

            existingSegmentModel.SequenceNumber = patchModel.SequenceNumber;

            return await UpsertAndRefreshSegmentModel(existingSegmentModel).ConfigureAwait(false);
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