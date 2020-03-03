using DFC.App.JobProfileOverview.Tests.IntegrationTests.API.Support.Interface;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.ContentType.JobProfile;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DFC.App.JobProfileOverview.Tests.IntegrationTests.API.Support
{
    public class CommonAction : IGeneralSupport, IJobProfileOverviewSupport
    {
        private static readonly Random Random = new Random();

        public string RandomString(int length)
        {
            return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ", length).Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public byte[] ConvertObjectToByteArray(object obj)
        {
            return Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(obj));
        }

        public T GetResource<T>(string resourceName)
        {
            var resourcesDirectory = Directory.CreateDirectory(System.Environment.CurrentDirectory).GetDirectories("Resource")[0];
            var files = resourcesDirectory.GetFiles();
            FileInfo selectedResource = null;

            for (var fileIndex = 0; fileIndex < files.Length; fileIndex++)
            {
                if (files[fileIndex].Name.ToUpperInvariant().StartsWith(resourceName.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase))
                {
                    selectedResource = files[fileIndex];
                    break;
                }
            }

            if (selectedResource == null)
            {
                throw new Exception($"No resource with the name {resourceName} was found");
            }

            using (var streamReader = new StreamReader(selectedResource.FullName))
            {
                var content = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(content);
            }
        }

        public SOCCodeContentType GenerateSOCCodeContentTypeForJobProfile(JobProfileContentType jobProfile)
        {
            return new SOCCodeContentType()
            {
                SOCCode = "12345",
                Id = jobProfile?.SocCodeData.Id,
                JobProfileId = jobProfile.JobProfileId,
                JobProfileTitle = jobProfile.Title,
                UrlName = jobProfile.SocCodeData.UrlName,
                Title = "12345",
                Description = "This record has been updated",
                ONetOccupationalCode = "12.1234-00",
                ApprenticeshipFramework = jobProfile.SocCodeData.ApprenticeshipFramework,
                ApprenticeshipStandards = jobProfile.SocCodeData.ApprenticeshipStandards,
            };
        }

        public SocCodeData GenerateSOCCodeJobProfileSection()
        {
            var socCode = this.RandomString(5);
            return new SocCodeData()
            {
                SOCCode = socCode,
                Id = Guid.NewGuid().ToString(),
                UrlName = socCode.ToUpperInvariant(),
                Description = "This record is the original record",
                ONetOccupationalCode = this.RandomString(5),
                ApprenticeshipFramework = new List<ApprenticeshipFramework>()
            {
                new ApprenticeshipFramework()
                {
                    Id = Guid.NewGuid().ToString(),
                    Description = this.RandomString(10),
                    Title = this.RandomString(10),
                    Url = new Uri($"https://{this.RandomString(10)}.com/"),
                },
            },
                ApprenticeshipStandards = new List<ApprenticeshipStandard>()
            {
                new ApprenticeshipStandard()
                {
                    Id = Guid.NewGuid().ToString(),
                    Description = this.RandomString(10),
                    Title = this.RandomString(10),
                    Url = new Uri($"https://{this.RandomString(10)}.com/"),
                },
            },
            };
        }

        public WorkingHoursDetail GenerateWorkingHoursDetailSection()
        {
            return new WorkingHoursDetail()
            {
                Id = Guid.NewGuid().ToString(),
                Description = "default-description",
                Title = "default-title",
                Url = new Uri($"https://{this.RandomString(10)}.com/"),
            };
        }

        public WorkingHoursDetailsClassification GenerateWorkingHoursDetailsClassificationForJobProfile(JobProfileContentType jobProfile)
        {
            return new WorkingHoursDetailsClassification()
            {   
                Id = jobProfile?.WorkingHoursDetails[0].Id,
                Description = "Updated description",
                JobProfileId = jobProfile.JobProfileId,
                JobProfileTitle = jobProfile.Title,
                Title = "Updated working hours detail title",
                Url = jobProfile.WorkingHoursDetails[0].Url,
            };
        }

        public WorkingPatternClassification GenerateWorkingPatternClassificationForJobProfile(JobProfileContentType jobProfile)
        {
            return new WorkingPatternClassification()
            {
                Id = jobProfile?.WorkingPattern[0].Id,
                Description = "Updated description",
                JobProfileId = jobProfile.JobProfileId,
                JobProfileTitle = jobProfile.Title,
                Title = "Updated working pattern title",
                Url = jobProfile.WorkingPattern[0].Url,
            };
        }

        public WorkingPattern GenerateWorkingPatternSection()
        {
            return new WorkingPattern()
            {
                Id = Guid.NewGuid().ToString(),
                Description = "default-description",
                Title = "default-title",
                Url = new Uri($"https://{this.RandomString(10)}.com/"),
            };
        }

        public WorkingPatternDetailClassification GenerateWorkingPatternDetailsClassificationForJobProfile(JobProfileContentType jobProfile)
        {
            return new WorkingPatternDetailClassification()
            {
                Id = jobProfile?.WorkingPatternDetails[0].Id,
                Description = "Updated description",
                JobProfileId = jobProfile.JobProfileId,
                JobProfileTitle = jobProfile.Title,
                Title = "Updated working pattern detail title",
                Url = jobProfile.WorkingPatternDetails[0].Url,
            };
        }

        public WorkingPatternDetail GenerateWorkingPatternDetailsSection()
        {
            return new WorkingPatternDetail()
            {
                Id = Guid.NewGuid().ToString(),
                Description = "default-description",
                Title = "default-title",
                Url = new Uri($"https://{this.RandomString(10)}.com/"),
            };
        }
    }
}
