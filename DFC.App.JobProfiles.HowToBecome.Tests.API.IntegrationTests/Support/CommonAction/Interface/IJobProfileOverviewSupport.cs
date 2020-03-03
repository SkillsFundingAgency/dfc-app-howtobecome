using DFC.App.JobProfileOverview.Tests.IntegrationTests.API.Model.Classification;
using DFC.App.JobProfileOverview.Tests.IntegrationTests.API.Model.ContentType;
using DFC.App.JobProfileOverview.Tests.IntegrationTests.API.Model.ContentType.JobProfile;

namespace DFC.App.JobProfileOverview.Tests.IntegrationTests.API.Support.Interface
{
    internal interface IJobProfileOverviewSupport
    {
        SocCodeData GenerateSOCCodeJobProfileSection();

        SOCCodeContentType GenerateSOCCodeContentTypeForJobProfile(JobProfileContentType jobProfile);

        WorkingHoursDetail GenerateWorkingHoursDetailSection();

        WorkingHoursDetailsClassification GenerateWorkingHoursDetailsClassificationForJobProfile(JobProfileContentType jobProfile);

        WorkingPatternClassification GenerateWorkingPatternClassificationForJobProfile(JobProfileContentType jobProfile);

        WorkingPattern GenerateWorkingPatternSection();

        WorkingPatternDetailClassification GenerateWorkingPatternDetailsClassificationForJobProfile(JobProfileContentType jobProfile);

        WorkingPatternDetail GenerateWorkingPatternDetailsSection();
    }
}
