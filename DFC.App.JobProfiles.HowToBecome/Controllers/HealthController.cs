using DFC.App.JobProfiles.HowToBecome.Extensions;
using DFC.App.JobProfiles.HowToBecome.SegmentService;
using DFC.App.JobProfiles.HowToBecome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.Controllers
{
    public class HealthController : Controller
    {
        private const string SuccessMessage = "Document store is available";

        private readonly ILogger<HealthController> logger;
        private readonly IHowToBecomeSegmentService howToBecomeSegmentService;
        private readonly string resourceName;

        public HealthController(ILogger<HealthController> logger, IHowToBecomeSegmentService howToBecomeSegmentService)
        {
            this.logger = logger;
            this.howToBecomeSegmentService = howToBecomeSegmentService;
            resourceName = typeof(Program).Namespace;
        }

        [HttpGet]
        [Route("health/ping")]
        public IActionResult Ping()
        {
            logger.LogInformation($"{nameof(Ping)} has been called");

            return Ok();
        }

        [HttpGet]
        [Route("health")]
        public async Task<IActionResult> Health()
        {
            logger.LogInformation($"{nameof(Health)} has been called");

            try
            {
                var isHealthy = await howToBecomeSegmentService.PingAsync().ConfigureAwait(false);
                if (isHealthy)
                {
                    logger.LogInformation($"{nameof(Health)} responded with: {resourceName} - {SuccessMessage}");

                    var viewModel = CreateHealthViewModel();

                    return this.NegotiateContentResult(viewModel);
                }

                logger.LogError($"{nameof(Health)}: Ping to {resourceName} has failed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(Health)}: {resourceName} exception: {ex.Message}");
            }

            return StatusCode((int)HttpStatusCode.ServiceUnavailable);
        }

        private HealthViewModel CreateHealthViewModel()
        {
            return new HealthViewModel
            {
                HealthItems = new List<HealthItemViewModel>
                {
                    new HealthItemViewModel
                    {
                        Service = resourceName,
                        Message = SuccessMessage,
                    },
                },
            };
        }
    }
}