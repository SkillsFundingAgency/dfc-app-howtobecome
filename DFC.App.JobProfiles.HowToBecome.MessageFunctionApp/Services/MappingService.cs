using AutoMapper;
using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels;
using Newtonsoft.Json;
using System.Linq;

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
            fullJobProfile.Data.Title = FormatHtbTitle(fullJobProfileMessage.Title, fullJobProfileMessage.WidgetContentTitle, fullJobProfileMessage.DynamicTitlePrefix );
            fullJobProfile.SequenceNumber = sequenceNumber;

            return fullJobProfile;
        }

        private string FormatHtbTitle(string title, string widgetContentTitle, string titlePrefix)
        {
            var changedTitle = string.IsNullOrEmpty(widgetContentTitle) ? title.ToLowerInvariant() : widgetContentTitle;
            return titlePrefix switch
            {
                "No Prefix" => $"{changedTitle}",
                "Prefix with a" => $"a {changedTitle}",
                "Prefix with an" => $"an {changedTitle}",
                "No Title" => string.Empty,
                _ => GetDefaultDynamicTitle(changedTitle),
            };
        }

        private string GetDefaultDynamicTitle(string title) => IsStartsWithVowel(title) ? $"an {title}" : $"a {title}";

        private bool IsStartsWithVowel(string title) => new[] { 'a', 'e', 'i', 'o', 'u' }.Contains(title.First());
    }
}