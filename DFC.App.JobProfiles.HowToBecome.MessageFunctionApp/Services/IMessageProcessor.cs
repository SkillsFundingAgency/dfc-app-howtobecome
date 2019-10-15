using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.Enums;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Services
{
    public interface IMessageProcessor
    {
        Task ProcessAsync(string message, long sequenceNumber, string sitefinityContentType, MessageAction eventType);
    }
}