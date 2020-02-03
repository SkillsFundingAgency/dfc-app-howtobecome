﻿using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support
{
    internal partial class CommonAction
    {
        //public async Task UpdateRouteRequirement(Topic topic, UpdateRouteRequirement updateRouteRequirement, RouteEntryType requirementType)
        //{
        //    Message updateMessage = new Message();
        //    updateMessage.ContentType = "application/json";
        //    updateMessage.Body = ConvertObjectToByteArray(updateRouteRequirement);
        //    updateMessage.CorrelationId = Guid.NewGuid().ToString();
        //    updateMessage.Label = "Automated route requirement message";
        //    updateMessage.MessageId = updateRouteRequirement.Id;
        //    updateMessage.UserProperties.Add("Id", updateRouteRequirement.JobProfileId);
        //    updateMessage.UserProperties.Add("ActionType", "Published");

        //    switch (requirementType)
        //    {
        //        case RouteEntryType.University:
        //            updateMessage.UserProperties.Add("CType", "UniversityEntryRequirements");
        //            break;

        //        case RouteEntryType.College:
        //            updateMessage.UserProperties.Add("CType", "CollegeEntryRequirements");
        //            break;

        //        case RouteEntryType.Apprenticeship:
        //            updateMessage.UserProperties.Add("CType", "ApprenticeshipEntryRequirements");
        //            break;
        //    }

        //    await topic.SendAsync(updateMessage);
        //}

        public RouteEntry GenerateRouteEntryForRouteEntryType(EnumLibrary.RouteEntryType routeEntryType)
        {
            return new RouteEntry()
            {
                RouteName = (int)routeEntryType,
                EntryRequirements = new List<EntryRequirement>(),
                MoreInformationLinks = new List<MoreInformationLink>(),
                FurtherRouteInformation = $"Default further information for the {routeEntryType} route entry type",
                RouteRequirement = $"Default route requirement for the {routeEntryType} route entry type",
                RouteSubjects = $"Default route subjects for the {routeEntryType} route entry type"
            };
        }

        public MoreInformationLink GenerateMoreInformationLinkSection(EnumLibrary.RouteEntryType routeEntryType)
        {
            return new MoreInformationLink()
            {
                Id = Guid.NewGuid().ToString(),
                Text = $"Default more information link for the {routeEntryType} route entry type",
                Title = $"Default more information title for the {routeEntryType} route entry type",
                Url = $"https://{RandomString(10)}.com"
            };
        }

        public Registration GenerateRegistrationsSection()
        {
            return new Registration()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Default registrations title",
                Info = "Default registrations info"
            };
        }

        public EntryRequirement GenerateEntryRequirementSection(EnumLibrary.RouteEntryType entryRequirementType)
        {
            return new EntryRequirement()
            {
                Id = Guid.NewGuid().ToString(),
                Info = $"Default {entryRequirementType} entry requirement",
                Title = $"Default {entryRequirementType} entry requirement title"
            };
        }

        public JobProfileContentType GenerateJobProfileContentType()
        {
            string canonicalName = RandomString(10);
            JobProfileContentType jobProfile = ResourceManager.GetResource<JobProfileContentType>("JobProfileContentType");
            jobProfile.JobProfileId = Guid.NewGuid().ToString();
            jobProfile.UrlName = canonicalName;
            jobProfile.CanonicalName = canonicalName;
            return jobProfile;
        }

        public async Task UpdateRegistration(Topic topic, RegistrationsContentType updateRegistration)
        {
            Message updateMessage = new Message();
            updateMessage.ContentType = "application/json";
            updateMessage.Body = ConvertObjectToByteArray(updateRegistration);
            updateMessage.CorrelationId = Guid.NewGuid().ToString();
            updateMessage.Label = "Automated registration message";
            updateMessage.MessageId = updateRegistration.Id;
            updateMessage.UserProperties.Add("Id", updateRegistration.JobProfileId);
            updateMessage.UserProperties.Add("ActionType", "Published");
            updateMessage.UserProperties.Add("CType", "Registration");
            await topic.SendAsync(updateMessage);
        }

        public async Task DeleteJobProfile(Topic topic, JobProfileContentType jobProfile)
        {
            JobProfileDeleteMessageBody messageBody = ResourceManager.GetResource<JobProfileDeleteMessageBody>("JobProfileDeleteMessageBody");
            messageBody.JobProfileId = jobProfile.JobProfileId;
            Message deleteMessage = CreateServiceBusMessage(jobProfile.JobProfileId, ConvertObjectToByteArray(messageBody), EnumLibrary.ContentType.JSON, ActionType.Deleted, CType.JobProfile);
            await topic.SendAsync(deleteMessage);
        }

        public async Task<JobProfileContentType> CreateJobProfile(Topic topic)
        {
            Guid messageId = Guid.NewGuid();
            string canonicalName = RandomString(10);
            JobProfileContentType messageBody = ResourceManager.GetResource<JobProfileContentType>("JobProfileContentType");
            RouteEntry UniversityRouteEntry = CreateARouteEntry(RouteEntryType.University);
            RouteEntry CollegeRouteEntry = CreateARouteEntry(RouteEntryType.College);
            RouteEntry ApprenticeshipRouteEntry = CreateARouteEntry(RouteEntryType.Apprenticeship);
            messageBody.HowToBecomeData.RouteEntries.Add(UniversityRouteEntry);
            messageBody.HowToBecomeData.RouteEntries.Add(CollegeRouteEntry);
            messageBody.HowToBecomeData.RouteEntries.Add(ApprenticeshipRouteEntry);
            messageBody.HowToBecomeData.Registrations[0].Id = Guid.NewGuid().ToString();
            messageBody.JobProfileId = messageId.ToString();
            messageBody.UrlName = canonicalName;
            messageBody.CanonicalName = canonicalName;
            Message message = CreateServiceBusMessage(messageId, ConvertObjectToByteArray(messageBody), EnumLibrary.ContentType.JSON, ActionType.Published, CType.JobProfile);
            await topic.SendAsync(message);
            return messageBody;
        }

        public async Task UpdateMoreInformationLinksForRequirementType(Topic topic, LinksContentType updateMoreInformationLink, RouteEntryType requirementType)
        {
            Message updateMessage = new Message();
            updateMessage.ContentType = "application/json";
            updateMessage.Body = ConvertObjectToByteArray(updateMoreInformationLink);
            updateMessage.CorrelationId = Guid.NewGuid().ToString();
            updateMessage.Label = "Automated more information link message";
            updateMessage.MessageId = updateMoreInformationLink.Id;
            updateMessage.UserProperties.Add("Id", updateMoreInformationLink.JobProfileId);
            updateMessage.UserProperties.Add("ActionType", "Published");

            switch (requirementType)
            {
                case RouteEntryType.University:
                    updateMessage.UserProperties.Add("CType", "UniversityLink");
                    break;

                case RouteEntryType.College:
                    updateMessage.UserProperties.Add("CType", "CollegeLink");
                    break;

                case RouteEntryType.Apprenticeship:
                    updateMessage.UserProperties.Add("CType", "ApprenticeshipLink");
                    break;
            }

            await topic.SendAsync(updateMessage);
        }

        public async Task UpdateEntryRequirementForRequirementType(Topic topic, EntryRequirementMessageBody updateEntryRequirement, RouteEntryType requirementType)
        {
            Message updateMessage = new Message();
            updateMessage.ContentType = "application/json";
            updateMessage.Body = ConvertObjectToByteArray(updateEntryRequirement);
            updateMessage.CorrelationId = Guid.NewGuid().ToString();
            updateMessage.Label = "Automated entry requirement message";
            updateMessage.MessageId = updateEntryRequirement.Id;
            updateMessage.UserProperties.Add("Id", updateEntryRequirement.JobProfileId);
            updateMessage.UserProperties.Add("ActionType", "Published");

            switch (requirementType)
            {
                case RouteEntryType.University:
                    updateMessage.UserProperties.Add("CType", "UniversityRequirement");
                    break;

                case RouteEntryType.College:
                    updateMessage.UserProperties.Add("CType", "CollegeRequirement");
                    break;

                case RouteEntryType.Apprenticeship:
                    updateMessage.UserProperties.Add("CType", "ApprenticeshipRequirement");
                    break;
            }

            await topic.SendAsync(updateMessage);
        }

        public EntryRequirementMessageBody CreateEntryRequirementMessageBody(string id, Guid jobProfileId, string info)
        {
            EntryRequirementMessageBody updateEntryRequirement = ResourceManager.GetResource<EntryRequirementMessageBody>("UpdateEntryRequirement");
            updateEntryRequirement.Id = id.ToString();
            updateEntryRequirement.JobProfileId = jobProfileId.ToString();
            updateEntryRequirement.Info = info;
            return updateEntryRequirement;
        }

        public EntryRequirement CreateEntryRequirement(string requirementInformation)
        {
            EntryRequirement entryRequirement = new EntryRequirement();
            entryRequirement.Id = Guid.NewGuid().ToString();
            entryRequirement.Title = "New entry requirement";
            entryRequirement.Info = requirementInformation;
            return entryRequirement;
        }

        public void AddEntryRequirementToRouteEntry(string entryRequirementInformation, RouteEntry routeEntry)
        {
            EntryRequirement entryRequirement = CreateEntryRequirement(entryRequirementInformation);
            routeEntry.EntryRequirements.Add(entryRequirement);
        }

        public RegistrationsContentType GenerateRegistrationUpdate(Guid id, Guid jobProfileId, string info)
        {
            RegistrationsContentType updateRegistration = ResourceManager.GetResource<RegistrationsContentType>("UpdateRegistration");
            updateRegistration.Id = id.ToString();
            updateRegistration.JobProfileId = jobProfileId.ToString();
            updateRegistration.Info = info;
            return updateRegistration;
        }

        public LinksContentType GenerateMoreInformationLinkUpdate(string id, Guid jobProfileId, string linkText)
        {
            LinksContentType updateMoreInformationLink = ResourceManager.GetResource<LinksContentType>("UpdateMoreInformationLinks");
            updateMoreInformationLink.Id = id;
            updateMoreInformationLink.JobProfileId = jobProfileId.ToString();
            updateMoreInformationLink.Text = linkText;
            return updateMoreInformationLink;
        }

        public void AddMoreInformationLinkToRouteEntry(string linkText, RouteEntry routeEntry)
        {
            MoreInformationLink moreInformationLink = CreateMoreInformationLink(linkText);
            routeEntry.MoreInformationLinks.Add(moreInformationLink);
        }

        public MoreInformationLink CreateMoreInformationLink(string linkText)
        {
            MoreInformationLink moreInformationLink = new MoreInformationLink();
            moreInformationLink.Id = Guid.NewGuid().ToString();
            moreInformationLink.Title = "New more information link";
            moreInformationLink.Url = "www.abc.com";
            moreInformationLink.Text = linkText;
            return moreInformationLink;
        }

        public RouteEntry CreateARouteEntry(RouteEntryType requirementType)
        {
            string htmlId;
            switch (requirementType)
            {
                case RouteEntryType.University:
                    htmlId = "universityRouteSubjects";
                    break;

                case RouteEntryType.College:
                    htmlId = "collegeRouteSubjects";
                    break;

                case RouteEntryType.Apprenticeship:
                    htmlId = "apprenticeshipsRouteSubjects";
                    break;

                default:
                    throw new ArgumentException("Unrecognised requirement type");
            }

            RouteEntry routeEntry = ResourceManager.GetResource<RouteEntry>("HowToBecomeRouteEntry");
            routeEntry.RouteName = (int)requirementType;
            AddEntryRequirementToRouteEntry("Requirement one", routeEntry);
            AddMoreInformationLinkToRouteEntry("More information link one", routeEntry);
            routeEntry.RouteSubjects = $"<div id='{htmlId}'><p>This is a paragraph for route subjects.</p><ul><li>Listed item</li></ul></div>";
            routeEntry.FurtherRouteInformation = "<p id='furtherRouteInformation'>Automated further information</p>";
            routeEntry.RouteRequirement = "";
            return routeEntry;
        }

        //public Dictionary<RouteEntryType, HowToBecomeRouteEntry> GetRouteEntriesFromHtmlResponse(Response<HtmlDocument> response)
        //{
        //    Dictionary<RouteEntryType, HowToBecomeRouteEntry> routeEntries = new Dictionary<RouteEntryType, HowToBecomeRouteEntry>();
        //    List<RouteEntryType> presentRequirementTypes = new List<RouteEntryType>();
        //    HtmlNode universitySection = response.Data.GetElementbyId("University");
        //    HtmlNode collegeSection = response.Data.GetElementbyId("College");
        //    HtmlNode apprenticeshipSection = response.Data.GetElementbyId("Apprenticeship");

        //    if (universitySection != null)
        //    {
        //        presentRequirementTypes.Add(RouteEntryType.University);
        //    }

        //    if (collegeSection != null)
        //    {
        //        presentRequirementTypes.Add(RouteEntryType.College);
        //    }

        //    if (apprenticeshipSection != null)
        //    {
        //        presentRequirementTypes.Add(RouteEntryType.Apprenticeship);
        //    }

        //    foreach (RouteEntryType requirementType in presentRequirementTypes)
        //    {
        //        string id = null;
        //        string xpathPrefix = null;

        //        switch (requirementType)
        //        {
        //            case RouteEntryType.University:
        //                id = "universityRouteSubjects";
        //                xpathPrefix = "//section[@id='University']";
        //                break;

        //            case RouteEntryType.College:
        //                id = "collegeRouteSubjects";
        //                xpathPrefix = "//section[@id='College']";
        //                break;

        //            case RouteEntryType.Apprenticeship:
        //                id = "apprenticeshipsRouteSubjects";
        //                xpathPrefix = "//section[@id='Apprenticeship']";
        //                break;
        //        }

        //        routeEntries.Add(requirementType, new HowToBecomeRouteEntry());
        //        routeEntries[requirementType].RouteSubjects = response.Data.DocumentNode.SelectSingleNode($"{xpathPrefix}//div[@id='{id}']").OuterHtml;
        //        routeEntries[requirementType].FurtherRouteInformation = response.Data.DocumentNode.SelectSingleNode($"{xpathPrefix}//p[@id='furtherRouteInformation']").OuterHtml;
        //        routeEntries[requirementType].RouteRequirement = response.Data.DocumentNode.SelectSingleNode($"{xpathPrefix}//p[2]").InnerText;
        //        HtmlNodeCollection entryRequirementsList = response.Data.DocumentNode.SelectNodes($"{xpathPrefix}//ul[@class='list-reqs']//li");
        //        routeEntries[requirementType].EntryRequirements = new List<EntryRequirement>();
        //        routeEntries[requirementType].EntryRequirements.Add(new EntryRequirement() { Info = entryRequirementsList[0].InnerText });
        //        HtmlNodeCollection moreInformationLinkList = response.Data.DocumentNode.SelectNodes($"{xpathPrefix}//ul[@class='list-link']//li");
        //        routeEntries[requirementType].MoreInformationLinks = new List<MoreInformationLink>();
        //        routeEntries[requirementType].MoreInformationLinks.Add(new MoreInformationLink() { Text = moreInformationLinkList[0].InnerText });
        //    }

        //    return routeEntries;
        //}

        //public UpdateRouteRequirement GenerateRouteRequirementContentTypeForJobProfile(Guid id, string title)
        //{
        //    UpdateRouteRequirement updateRouteRequirement = ResourceManager.GetResource<UpdateRouteRequirement>("UpdateRouteRequirement");
        //    updateRouteRequirement.Id = id.ToString();
        //    updateRouteRequirement.Title = title;
        //    return updateRouteRequirement;
        //}

        public EntryRequirementsClassification GenerateEntryRequirementsClassificationForJobProfile(RouteEntryType routeEntryType, JobProfileContentType jobProfile)
        {
            return new EntryRequirementsClassification()
            {
                Id = jobProfile.HowToBecomeData.RouteEntries[(int)routeEntryType].EntryRequirements[0].Id,
                Description = $"This is an updated description for the entry requirement for the {routeEntryType.ToString()} route entry",
                Title = $"This is an updated title for the entry requirement for the {routeEntryType.ToString()} route entry",
                Url = $"https://{RandomString(10)}.com/",
                JobProfileId = jobProfile.JobProfileId,
                JobProfileTitle = jobProfile.Title
            };
        }

        public RegistrationsContentType GenerateRegistrationsContentTypeForJobProfile(JobProfileContentType jobProfile)
        {
            return new RegistrationsContentType()
            {
                Id = jobProfile.HowToBecomeData.Registrations[0].Id,
                Info = "This is the upated info for the registrations record",
                JobProfileId = jobProfile.JobProfileId,
                JobProfileTitle = jobProfile.Title,
                Title = "This is the upated title for the registrations record"
            };
        }

        public LinksContentType GenerateLinksContentTypeForJobProfile(RouteEntryType routeEntryType, JobProfileContentType jobProfile)
        {
            RouteEntry routeEntry = jobProfile.HowToBecomeData.RouteEntries.Where(re => re.RouteName.Equals((int)routeEntryType)).FirstOrDefault();

            if(routeEntry == null)
            {
                throw new Exception($"Unable to find the route entry with route name {(int)routeEntryType}");
            }

            return new LinksContentType()
            {
                Id = routeEntry.MoreInformationLinks[0].Id,
                Text = "This is updated link text",
                Title = "This is an updated link title",
                Url = $"https://{RandomString(10)}.com/",
                JobProfileId = jobProfile.JobProfileId,
                JobProfileTitle = jobProfile.Title
            };
        }
    }
}
