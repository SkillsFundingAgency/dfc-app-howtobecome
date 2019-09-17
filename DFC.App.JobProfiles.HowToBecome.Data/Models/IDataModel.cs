using Newtonsoft.Json;
using System;

namespace DFC.App.JobProfiles.HowToBecome.Data.Models
{
    public interface IDataModel
    {
        [JsonProperty(PropertyName = "id")]
        Guid DocumentId { get; set; }

        int PartitionKey { get; }
    }
}