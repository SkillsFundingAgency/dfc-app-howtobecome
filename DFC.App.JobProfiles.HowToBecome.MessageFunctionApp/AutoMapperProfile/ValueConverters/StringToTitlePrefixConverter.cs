using AutoMapper;
using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using System;

namespace DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.AutoMapperProfile.ValueConverters
{
    public class StringToTitlePrefixConverter : IValueConverter<string, TitlePrefix>
    {
        public TitlePrefix Convert(string sourceMember, ResolutionContext context)
        {
            if (Enum.TryParse<TitlePrefix>(sourceMember, out var result))
            {
                return result;
            }

            return TitlePrefix.AsDefined;
        }
    }
}
