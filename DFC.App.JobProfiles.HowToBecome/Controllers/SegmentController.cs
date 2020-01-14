using DFC.App.JobProfiles.HowToBecome.ApiModels;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels;
using DFC.App.JobProfiles.HowToBecome.Extensions;
using DFC.App.JobProfiles.HowToBecome.SegmentService;
using DFC.App.JobProfiles.HowToBecome.ViewModels;
using DFC.Logger.AppInsights.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.Controllers
{
    public class SegmentController : Controller
    {
        private const string IndexActionName = nameof(Index);
        private const string DocumentActionName = nameof(Document);
        private const string BodyActionName = nameof(Body);
        private const string PostActionName = nameof(Post);
        private const string PutActionName = nameof(Put);
        private const string DeleteActionName = nameof(Delete);
        private const string PatchLinksActionName = nameof(PatchLinks);
        private const string PatchRequirementsActionName = nameof(PatchRequirements);
        private const string PatchSimpleClassificationActionName = nameof(PatchEntryRequirement);
        private const string PatchRegistrationActionName = nameof(PatchRegistration);

        private readonly ILogService logService;
        private readonly IHowToBecomeSegmentService howToBecomeSegmentService;
        private readonly AutoMapper.IMapper mapper;

        public SegmentController(ILogService logService, IHowToBecomeSegmentService howToBecomeSegmentService, AutoMapper.IMapper mapper)
        {
            this.logService = logService;
            this.howToBecomeSegmentService = howToBecomeSegmentService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("segment")]
        public async Task<IActionResult> Index()
        {
            logService.LogInformation($"{IndexActionName} has been called");

            var viewModel = new IndexViewModel();
            var howToBecomeSegmentModels = await howToBecomeSegmentService.GetAllAsync().ConfigureAwait(false);

            if (howToBecomeSegmentModels != null)
            {
                viewModel.Documents = (from a in howToBecomeSegmentModels.OrderBy(o => o.CanonicalName)
                                       select mapper.Map<IndexDocumentViewModel>(a)).ToList();

                logService.LogInformation($"{IndexActionName} has succeeded");
            }
            else
            {
                logService.LogWarning($"{IndexActionName} has returned with no results");
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("segment/{article}")]
        public async Task<IActionResult> Document(string article)
        {
            logService.LogInformation($"{DocumentActionName} has been called with: {article}");

            var howToBecomeSegmentModel = await howToBecomeSegmentService.GetByNameAsync(article).ConfigureAwait(false);

            if (howToBecomeSegmentModel != null)
            {
                var viewModel = mapper.Map<DocumentViewModel>(howToBecomeSegmentModel);

                logService.LogInformation($"{DocumentActionName} has succeeded for: {article}");

                return View(viewModel);
            }

            logService.LogWarning($"{DocumentActionName} has returned no content for: {article}");

            return NoContent();
        }

        [HttpGet]
        [Route("segment/{documentId}/contents")]
        public async Task<IActionResult> Body(Guid documentId)
        {
            logService.LogInformation($"{BodyActionName} has been called with: {documentId}");

            var howToBecomeSegmentModel = await howToBecomeSegmentService.GetByIdAsync(documentId).ConfigureAwait(false);
            if (howToBecomeSegmentModel != null)
            {
                var viewModel = mapper.Map<DocumentViewModel>(howToBecomeSegmentModel);

                logService.LogInformation($"{BodyActionName} has succeeded for: {documentId}");

                var apiModel = mapper.Map<HowToBecomeApiModel>(howToBecomeSegmentModel.Data);

                return this.NegotiateContentResult(viewModel, apiModel);
            }

            logService.LogWarning($"{BodyActionName} has returned no content for: {documentId}");

            return NoContent();
        }

        [HttpPost]
        [Route("segment")]
        public async Task<IActionResult> Post([FromBody]HowToBecomeSegmentModel upsertHowToBecomeSegmentModel)
        {
            logService.LogInformation($"{PostActionName} has been called");

            if (upsertHowToBecomeSegmentModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingDocument = await howToBecomeSegmentService.GetByIdAsync(upsertHowToBecomeSegmentModel.DocumentId).ConfigureAwait(false);
            if (existingDocument != null)
            {
                return new StatusCodeResult((int)HttpStatusCode.AlreadyReported);
            }

            var response = await howToBecomeSegmentService.UpsertAsync(upsertHowToBecomeSegmentModel)
                .ConfigureAwait(false);

            logService.LogInformation($"{PostActionName} has upserted content for: {upsertHowToBecomeSegmentModel.CanonicalName}");

            return new StatusCodeResult((int)response);
        }

        [HttpPut]
        [Route("segment")]
        public async Task<IActionResult> Put([FromBody]HowToBecomeSegmentModel upsertHowToBecomeSegmentModel)
        {
            logService.LogInformation($"{PutActionName} has been called");

            if (upsertHowToBecomeSegmentModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingDocument = await howToBecomeSegmentService.GetByIdAsync(upsertHowToBecomeSegmentModel.DocumentId).ConfigureAwait(false);
            if (existingDocument == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }

            if (upsertHowToBecomeSegmentModel.SequenceNumber <= existingDocument.SequenceNumber)
            {
                return new StatusCodeResult((int)HttpStatusCode.AlreadyReported);
            }

            upsertHowToBecomeSegmentModel.Etag = existingDocument.Etag;
            upsertHowToBecomeSegmentModel.SocLevelTwo = existingDocument.SocLevelTwo;

            var response = await howToBecomeSegmentService.UpsertAsync(upsertHowToBecomeSegmentModel).ConfigureAwait(false);

            return new StatusCodeResult((int)response);
        }

        [HttpPatch]
        [Route("segment/{documentId}/links")]
        public async Task<IActionResult> PatchLinks([FromBody]PatchLinksModel patchLinksModel, Guid documentId)
        {
            logService.LogInformation($"{PatchLinksActionName} has been called");

            if (patchLinksModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await howToBecomeSegmentService.PatchLinksAsync(patchLinksModel, documentId).ConfigureAwait(false);
            if (response != HttpStatusCode.OK && response != HttpStatusCode.Created)
            {
                logService.LogError($"{PatchLinksActionName}: Error while patching Link content for Job Profile with Id: {patchLinksModel.JobProfileId} for the {patchLinksModel.RouteName.ToString()} link");
            }

            return new StatusCodeResult((int)response);
        }

        [HttpPatch]
        [Route("segment/{documentId}/requirements")]
        public async Task<IActionResult> PatchRequirements([FromBody]PatchRequirementsModel patchRequirementsModel, Guid documentId)
        {
            logService.LogInformation($"{PatchRequirementsActionName} has been called");

            if (patchRequirementsModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await howToBecomeSegmentService.PatchRequirementsAsync(patchRequirementsModel, documentId).ConfigureAwait(false);
            if (response != HttpStatusCode.OK && response != HttpStatusCode.Created)
            {
                logService.LogError($"{PatchRequirementsActionName}: Error while patching Requirement content for Job Profile with Id: {patchRequirementsModel.JobProfileId} for the {patchRequirementsModel.RouteName.ToString()} link");
            }

            return new StatusCodeResult((int)response);
        }

        [HttpPatch]
        [Route("segment/{documentId}/entryRequirement")]
        public async Task<IActionResult> PatchEntryRequirement([FromBody]PatchSimpleClassificationModel patchSimpleClassificationModel, Guid documentId)
        {
            logService.LogInformation($"{PatchSimpleClassificationActionName} has been called");

            if (patchSimpleClassificationModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await howToBecomeSegmentService.PatchSimpleClassificationAsync(patchSimpleClassificationModel, documentId).ConfigureAwait(false);
            if (response != HttpStatusCode.OK && response != HttpStatusCode.Created)
            {
                logService.LogError($"{PatchSimpleClassificationActionName}: Error while patching Entry Requirement content for Job Profile with Id: {patchSimpleClassificationModel.JobProfileId} for the {patchSimpleClassificationModel.RouteName.ToString()} link");
            }

            return new StatusCodeResult((int)response);
        }

        [HttpPatch]
        [Route("segment/{documentId}/registration")]
        public async Task<IActionResult> PatchRegistration([FromBody]PatchRegistrationModel patchRegistrationModel, Guid documentId)
        {
            logService.LogInformation($"{PatchSimpleClassificationActionName} has been called");

            if (patchRegistrationModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await howToBecomeSegmentService.PatchRegistrationAsync(patchRegistrationModel, documentId).ConfigureAwait(false);
            if (response != HttpStatusCode.OK && response != HttpStatusCode.Created)
            {
                logService.LogError($"{PatchRegistrationActionName}: Error while patching Registration content for Job Profile with Id: {patchRegistrationModel.JobProfileId} for the {patchRegistrationModel.RouteName.ToString()} link");
            }

            return new StatusCodeResult((int)response);
        }

        [HttpDelete]
        [Route("segment/{documentId}")]
        public async Task<IActionResult> Delete(Guid documentId)
        {
            logService.LogInformation($"{DeleteActionName} has been called");

            var isDeleted = await howToBecomeSegmentService.DeleteAsync(documentId).ConfigureAwait(false);
            if (isDeleted)
            {
                logService.LogInformation($"{DeleteActionName} has deleted content for document Id: {documentId}");
                return Ok();
            }
            else
            {
                logService.LogWarning($"{DeleteActionName} has returned no content for: {documentId}");
                return NotFound();
            }
        }
    }
}