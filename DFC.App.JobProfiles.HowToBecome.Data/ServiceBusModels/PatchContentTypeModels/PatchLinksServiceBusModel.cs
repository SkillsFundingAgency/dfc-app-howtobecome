using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.PatchContentTypeModels
{
    public class PatchLinksServiceBusModel : BaseJobProfileMessage
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Text { get; set; }

        [Required]
        public Uri Url { get; set; }
    }
}