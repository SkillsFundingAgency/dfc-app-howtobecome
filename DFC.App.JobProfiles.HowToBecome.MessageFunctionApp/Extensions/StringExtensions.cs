using System;

namespace DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.Extensions
{
    public static class StringExtensions
    {
        public static string GetFormattedUrl(this string endpoint, Uri baseAddress, Guid? id = null)
        {
            var substitutedEndpoint = endpoint?.Replace("{0}", id.ToString().ToLowerInvariant(), System.StringComparison.OrdinalIgnoreCase);
            return $"{baseAddress}{substitutedEndpoint}";
        }
    }
}