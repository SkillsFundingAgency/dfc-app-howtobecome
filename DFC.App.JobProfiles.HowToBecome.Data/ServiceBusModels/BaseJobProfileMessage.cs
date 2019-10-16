using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels
{
    public class BaseJobProfileMessage
    {
        [Required]
        public Guid JobProfileId { get; set; }
    }
}