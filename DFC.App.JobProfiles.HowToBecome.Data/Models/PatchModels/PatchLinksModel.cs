using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels
{
    public class PatchLinksModel : BasePatchModel
    {
        [Required]
        public string Title { get; set; }

        public string Text { get; set; }

        public Uri Url { get; set; }
    }
}