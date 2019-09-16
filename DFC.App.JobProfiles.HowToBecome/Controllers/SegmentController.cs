using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Extensions;
using DFC.App.JobProfiles.HowToBecome.SegmentService;
using DFC.App.JobProfiles.HowToBecome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private const string SaveActionName = nameof(Save);
        private const string DeleteActionName = nameof(Delete);

        private readonly ILogger<SegmentController> logger;
        private readonly IHowToBecomeSegmentService howToBecomeSegmentService;
        private readonly AutoMapper.IMapper mapper;

        public SegmentController(ILogger<SegmentController> logger, IHowToBecomeSegmentService howToBecomeSegmentService, AutoMapper.IMapper mapper)
        {
            this.logger = logger;
            this.howToBecomeSegmentService = howToBecomeSegmentService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("/")]
        [Route("segment")]
        public async Task<IActionResult> Index()
        {
            logger.LogInformation($"{IndexActionName} has been called");

            var viewModel = new IndexViewModel();
            var howToBecomeSegmentModels = await howToBecomeSegmentService.GetAllAsync().ConfigureAwait(false);

            if (howToBecomeSegmentModels != null)
            {
                viewModel.Documents = (from a in howToBecomeSegmentModels.OrderBy(o => o.CanonicalName)
                                       select mapper.Map<IndexDocumentViewModel>(a)).ToList();

                logger.LogInformation($"{IndexActionName} has succeeded");
            }
            else
            {
                logger.LogWarning($"{IndexActionName} has returned with no results");
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("segment/{article}")]
        public async Task<IActionResult> Document(string article)
        {
            logger.LogInformation($"{DocumentActionName} has been called with: {article}");

            var howToBecomeSegmentModel = await howToBecomeSegmentService.GetByNameAsync(article, Request.IsDraftRequest()).ConfigureAwait(false);

            if (howToBecomeSegmentModel != null)
            {
                var viewModel = mapper.Map<DocumentViewModel>(howToBecomeSegmentModel);

                logger.LogInformation($"{DocumentActionName} has succeeded for: {article}");

                return View(viewModel);
            }

            logger.LogWarning($"{DocumentActionName} has returned no content for: {article}");

            return NoContent();
        }

        [HttpGet]
        [Route("segment/{article}/contents")]
        public async Task<IActionResult> Body(string article)
        {
            logger.LogInformation($"{BodyActionName} has been called with: {article}");

            var howToBecomeSegmentModel = await howToBecomeSegmentService.GetByNameAsync(article, Request.IsDraftRequest()).ConfigureAwait(false);
            if (howToBecomeSegmentModel != null)
            {
                var viewModel = mapper.Map<DocumentViewModel>(howToBecomeSegmentModel);

                logger.LogInformation($"{BodyActionName} has succeeded for: {article}");

                return this.NegotiateContentResult(viewModel, howToBecomeSegmentModel.Data);
            }

            logger.LogWarning($"{BodyActionName} has returned no content for: {article}");

            return NoContent();
        }

        [HttpPut]
        [HttpPost]
        [Route("segment")]
        public async Task<IActionResult> Save([FromBody]HowToBecomeSegmentModel upsertHowToBecomeSegmentModel)
        {
            logger.LogInformation($"{SaveActionName} has been called");

            if (upsertHowToBecomeSegmentModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await howToBecomeSegmentService.UpsertAsync(upsertHowToBecomeSegmentModel)
                .ConfigureAwait(false);

            if (response.ResponseStatusCode == HttpStatusCode.Created)
            {
                logger.LogInformation($"{SaveActionName} has created content for: {upsertHowToBecomeSegmentModel.CanonicalName}");

                return new CreatedAtActionResult(
                    SaveActionName,
                    "Segment",
                    new { article = response.HowToBecomeSegmentModel.CanonicalName },
                    response.HowToBecomeSegmentModel);
            }
            else
            {
                logger.LogInformation($"{SaveActionName} has updated content for: {upsertHowToBecomeSegmentModel.CanonicalName}");

                return new OkObjectResult(response.HowToBecomeSegmentModel);
            }
        }

        [HttpDelete]
        [Route("segment/{documentId}")]
        public async Task<IActionResult> Delete(Guid documentId)
        {
            logger.LogInformation($"{DeleteActionName} has been called");

            var isDeleted = await howToBecomeSegmentService.DeleteAsync(documentId).ConfigureAwait(false);
            if (isDeleted)
            {
                logger.LogInformation($"{DeleteActionName} has deleted content for document Id: {documentId}");
                return Ok();
            }
            else
            {
                logger.LogWarning($"{DeleteActionName} has returned no content for: {documentId}");
                return NotFound();
            }
        }
    }
}