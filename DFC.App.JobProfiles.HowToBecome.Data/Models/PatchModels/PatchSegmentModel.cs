using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels
{
    public class PatchSegmentModel
    {
        [Required]
        public Guid JobProfileId { get; set; }

        public string CanonicalName { get; set; }

        public string SocLevelTwo { get; set; }

        public HowToBecomeSegmentDataModel Data { get; set; }
    }
}