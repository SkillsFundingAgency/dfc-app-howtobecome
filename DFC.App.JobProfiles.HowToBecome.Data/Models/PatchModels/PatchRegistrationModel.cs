﻿using System.ComponentModel.DataAnnotations;

namespace DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels
{
    public class PatchRegistrationModel : BasePatchModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Info { get; set; }
    }
}