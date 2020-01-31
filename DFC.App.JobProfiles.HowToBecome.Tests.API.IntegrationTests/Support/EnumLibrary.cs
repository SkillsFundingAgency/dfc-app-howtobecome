using System.ComponentModel;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support
{
    internal class EnumLibrary
    {
        public enum RouteEntryType
        {
            University = 2,
            College = 1,
            Apprenticeship = 0
        }

        public enum CType
        {
            JobProfile,
            UniversityLink,
            CollegeLink,
            ApprenticeshipLink,
            UniversityRequirement,
            CollegeRequirement,
            ApprenticeshipRequirement,
            UniversityEntryRequirements,
            CollegeEntryRequirements,
            ApprenticeshipEntryRequirements,
            Registration
        }

        public enum ActionType
        {
            Published,
            Deleted
        }

        public enum ContentType
        {
            [Description("application/json")]
            JSON,
            [Description("text/html")]
            HTML
        }
    }
}
