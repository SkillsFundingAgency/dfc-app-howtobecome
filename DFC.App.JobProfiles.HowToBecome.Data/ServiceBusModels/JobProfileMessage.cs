using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels
{
    public class JobProfileMessage : BaseJobProfileMessage
    {
        [Required]
        public string SocLevelTwo { get; set; }

        public DateTime LastModified { get; set; }

        public string Title { get; set; }

        public string DynamicTitlePrefix { get; set; }

        [Required]
        public string CanonicalName { get; set; }

        [Required]
        public SitefinityHowToBecomeMessage HowToBecomeData { get; set; }
    }
}