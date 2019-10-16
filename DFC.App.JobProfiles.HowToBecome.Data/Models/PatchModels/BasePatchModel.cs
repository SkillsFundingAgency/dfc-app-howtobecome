using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels
{
    public class BasePatchModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid JobProfileId { get; set; }

        [Required]
        public MessageAction EventType { get; set; }

        [Required]
        public RouteName RouteName { get; set; }

        [Required]
        public long SequenceNumber { get; set; }
    }
}