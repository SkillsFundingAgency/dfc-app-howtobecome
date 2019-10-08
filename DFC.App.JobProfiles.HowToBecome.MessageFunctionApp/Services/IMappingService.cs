using DFC.App.JobProfiles.HowToBecome.Data.Models;

namespace DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Services
{
    public interface IMappingService
    {
        HowToBecomeSegmentModel MapToSegmentModel(string message, long sequenceNumber);
    }
}