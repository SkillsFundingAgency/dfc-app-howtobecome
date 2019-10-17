using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.PatchContentTypeModels
{
    public class PatchRegistrationsServiceBusModel : BaseJobProfileMessage
    {
        [Required]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Info { get; set; }
    }
}