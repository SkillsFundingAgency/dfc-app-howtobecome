using DFC.App.JobProfiles.HowToBecome.Data.Models;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.DraftSegmentService
{
    public interface IDraftHowToBecomeSegmentService
    {
        Task<HowToBecomeSegmentModel> GetSitefinityData(string canonicalName);
    }
}