using System.ComponentModel.DataAnnotations;

namespace DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels
{
    public class PatchSimpleClassificationModel : BasePatchModel
    {
        [Required]
        public string Title { get; set; }
    }
}