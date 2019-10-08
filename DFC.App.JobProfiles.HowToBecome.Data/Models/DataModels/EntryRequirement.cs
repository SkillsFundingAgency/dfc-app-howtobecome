using System;

namespace DFC.App.JobProfiles.HowToBecome.Data.Models.DataModels
{
    public class EntryRequirement
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Rank { get; set; }
    }
}