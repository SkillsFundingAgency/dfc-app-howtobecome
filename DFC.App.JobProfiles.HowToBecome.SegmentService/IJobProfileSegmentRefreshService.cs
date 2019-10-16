using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.SegmentService
{
    public interface IJobProfileSegmentRefreshService<in TModel>
    {
        Task SendMessageAsync(TModel model);
    }
}