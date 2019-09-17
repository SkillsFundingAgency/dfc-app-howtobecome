using System.Net;

namespace DFC.App.JobProfiles.HowToBecome.Data.Models
{
    public class UpsertHowToBecomeSegmentModelResponse
    {
        public HowToBecomeSegmentModel HowToBecomeSegmentModel { get; set; }

        public HttpStatusCode ResponseStatusCode { get; set; }
    }
}