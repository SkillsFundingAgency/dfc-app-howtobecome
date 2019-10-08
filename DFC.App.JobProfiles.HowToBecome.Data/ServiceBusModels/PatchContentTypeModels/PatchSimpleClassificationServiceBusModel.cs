using System.ComponentModel.DataAnnotations;

namespace DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.PatchContentTypeModels
{
    public class PatchSimpleClassificationServiceBusModel : BaseJobProfileMessage
    {
        [Required]
        public string Title { get; set; }
    }
}