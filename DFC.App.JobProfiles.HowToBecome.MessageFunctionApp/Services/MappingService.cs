using AutoMapper;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels;
using Newtonsoft.Json;

namespace DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Services
{
    public class MappingService : IMappingService
    {
        private readonly IMapper mapper;

        public MappingService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public HowToBecomeSegmentModel MapToSegmentModel(string message, long sequenceNumber)
        {
            var fullJobProfileMessage = JsonConvert.DeserializeObject<JobProfileMessage>(message);
            var fullJobProfile = mapper.Map<HowToBecomeSegmentModel>(fullJobProfileMessage);
            fullJobProfile.SequenceNumber = sequenceNumber;

            return fullJobProfile;
        }
    }
}