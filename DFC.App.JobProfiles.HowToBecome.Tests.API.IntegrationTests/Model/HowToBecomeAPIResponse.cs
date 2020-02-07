using System.Collections.Generic;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model
{
    class HowToBecomeAPIResponse
    {
        public List<string> entryRouteSummary { get; set; }

        public EntryRoutes entryRoutes { get; set; }

        public MoreInformation moreInformation { get; set; }
    }

    public class University
    {
        public List<string> relevantSubjects { get; set; }

        public List<string> furtherInformation { get; set; }

        public string entryRequirementPreface { get; set; }

        public List<string> entryRequirements { get; set; }

        public List<string> additionalInformation { get; set; }
    }

    public class College
    {
        public List<string> relevantSubjects { get; set; }

        public List<string> furtherInformation { get; set; }

        public string entryRequirementPreface { get; set; }

        public List<string> entryRequirements { get; set; }

        public List<string> additionalInformation { get; set; }
    }

    public class Apprenticeship
    {
        public List<string> relevantSubjects { get; set; }

        public List<object> furtherInformation { get; set; }

        public string entryRequirementPreface { get; set; }

        public List<string> entryRequirements { get; set; }

        public List<string> additionalInformation { get; set; }
    }

    public class EntryRoutes
    {
        public University university { get; set; }

        public College college { get; set; }

        public Apprenticeship apprenticeship { get; set; }

        public List<string> work { get; set; }

        public List<object> volunteering { get; set; }

        public List<object> directApplication { get; set; }

        public List<string> otherRoutes { get; set; }
    }

    public class MoreInformation
    {
        public List<object> registrations { get; set; }

        public List<string> careerTips { get; set; }

        public List<string> professionalAndIndustryBodies { get; set; }

        public List<string> furtherInformation { get; set; }
    }
}
