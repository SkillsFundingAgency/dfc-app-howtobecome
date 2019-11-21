using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels
{
    public class HowToBecomeDeleteServiceBusModel
    {
        [Required]
        public Guid JobProfileId { get; set; }

        [Required]
        public DateTime LastReviewed { get; set; }
    }
}