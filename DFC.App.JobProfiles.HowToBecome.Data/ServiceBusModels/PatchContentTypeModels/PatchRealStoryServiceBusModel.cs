using DFC.App.JobProfiles.HowToBecome.Data.Models.DataModels;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.PatchContentTypeModels
{
    public class PatchRealStoryServiceBusModel : BaseJobProfileMessage
    {
        [Required]
        public RealStory RealStory { get; set; }
    }
}