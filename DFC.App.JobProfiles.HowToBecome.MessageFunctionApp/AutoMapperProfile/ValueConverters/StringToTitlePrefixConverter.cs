using AutoMapper;
using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.AutoMapperProfile.ValueConverters
{
    [ExcludeFromCodeCoverage]
    public class StringToTitlePrefixConverter : IValueConverter<string, TitlePrefix>
    {
        public TitlePrefix Convert(string sourceMember, ResolutionContext context)
        {
            if (!string.IsNullOrWhiteSpace(sourceMember))
            {
                sourceMember = sourceMember.Replace(" ", string.Empty, StringComparison.OrdinalIgnoreCase);

                if (Enum.TryParse<TitlePrefix>(sourceMember, true, out var result))
                {
                    return result;
                }
            }

            return TitlePrefix.AsDefined;
        }
    }
}
