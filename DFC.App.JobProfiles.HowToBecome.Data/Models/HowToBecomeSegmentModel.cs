using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.JobProfiles.HowToBecome.Data.Models
{
    public class HowToBecomeSegmentModel : IDataModel
    {
        [Required]
        [JsonProperty(PropertyName = "id")]
        public Guid DocumentId { get; set; }

        [JsonProperty(PropertyName = "_etag")]
        public string Etag { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Updated { get; set; }

        public string PartitionKey => SocLevelTwo?.Substring(0, 2);

        [Required]
        public string SocLevelTwo { get; set; }

        [Required]
        public string CanonicalName { get; set; }

        public HowToBecomeSegmentDataModel Data { get; set; }
    }
}