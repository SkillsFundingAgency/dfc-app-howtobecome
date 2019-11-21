using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.PatchContentTypeModels
{
    public class PatchSimpleClassificationServiceBusModel : BaseJobProfileMessage
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }
    }
}