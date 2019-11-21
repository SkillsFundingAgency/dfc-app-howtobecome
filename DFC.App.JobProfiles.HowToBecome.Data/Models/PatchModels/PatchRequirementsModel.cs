using System.ComponentModel.DataAnnotations;

namespace DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels
{
    public class PatchRequirementsModel : BasePatchModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Info { get; set; }

        [Required]
        public int Rank { get; set; }
    }
}