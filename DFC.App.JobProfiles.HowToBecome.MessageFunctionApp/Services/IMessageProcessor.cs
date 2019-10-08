using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.Enums;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Services
{
    public interface IMessageProcessor
    {
        Task ProcessUpdateMessage(string message, long sequenceNumber, string sitefinityContentType, EventType eventType);
    }
}