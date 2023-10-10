using DFC.App.JobProfiles.HowToBecome.Data.Models.DataModels;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels
{
    public class PatchRealStoryModel : BasePatchModel
    {
        [Required]
        public RealStory RealStory { get; set; }
    }
}