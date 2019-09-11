﻿using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.JobProfiles.HowToBecome.Data.Models
{
    public class HowToBecomeSegmentModel : IDataModel
    {
        [JsonProperty(PropertyName = "id")]
        public Guid DocumentId { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Updated { get; set; }

        public int PartitionKey => Created.Second;

        [Required]
        public string CanonicalName { get; set; }

        public string Title { get; set; }

        public string Markup { get; set; }

        public HowToBecomeSegmentDataModel Data { get; set; }
    }
}