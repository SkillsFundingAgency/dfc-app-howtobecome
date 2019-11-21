using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.PatchContentTypeModels
{
    public class PatchRequirementsServiceBusModel : BaseJobProfileMessage
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Info { get; set; }

        [Required]
        public int Rank { get; set; }
    }
}