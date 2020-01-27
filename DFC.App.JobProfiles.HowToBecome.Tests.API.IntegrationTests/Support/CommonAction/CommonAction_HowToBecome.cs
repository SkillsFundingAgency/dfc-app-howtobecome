using DFC.Api.JobProfiles.Common.APISupport;
using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.Interface;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support
{
    internal partial class CommonAction : IHowToBecomeSupport
    {
        public async Task UpdateRouteRequirement(Topic topic, UpdateRouteRequirement updateRouteRequirement, RequirementType requirementType)
        {
            Message updateMessage = new Message();
            updateMessage.ContentType = "application/json";
            updateMessage.Body = ConvertObjectToByteArray(updateRouteRequirement);
            updateMessage.CorrelationId = Guid.NewGuid().ToString();
            updateMessage.Label = "Automated route requirement message";
            updateMessage.MessageId = updateRouteRequirement.Id;
            updateMessage.UserProperties.Add("Id", updateRouteRequirement.JobProfileId);
            updateMessage.UserProperties.Add("ActionType", "Published");

            switch (requirementType)
            {
                case RequirementType.University:
                    updateMessage.UserProperties.Add("CType", "UniversityEntryRequirements");
                    break;

                case RequirementType.College:
                    updateMessage.UserProperties.Add("CType", "CollegeEntryRequirements");
                    break;

                case RequirementType.Apprenticeship:
                    updateMessage.UserProperties.Add("CType", "ApprenticeshipEntryRequirements");
                    break;
            }

            await topic.SendAsync(updateMessage);
        }

        public async Task UpdateRegistration(Topic topic, UpdateRegistration updateRegistration)
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

        public async Task DeleteJobProfileWithId(Topic topic, Guid jobProfileId)
        {
            JobProfileDeleteMessageBody messageBody = ResourceManager.GetResource<JobProfileDeleteMessageBody>("JobProfileDeleteMessageBody");
            messageBody.JobProfileId = jobProfileId.ToString();
            Message deleteMessage = CreateServiceBusMessage(jobProfileId, ConvertObjectToByteArray(messageBody), EnumLibrary.ContentType.JSON, ActionType.Deleted, CType.JobProfile);
            await topic.SendAsync(deleteMessage);
        }

        public async Task CreateJobProfileWithRouteEntries(Topic topic, Guid messageId, Guid registrationId, string canonicalName, List<RouteEntry> routeEntries)
        {
            JobProfileCreateMessageBody messageBody = ResourceManager.GetResource<JobProfileCreateMessageBody>("JobProfileCreateMessageBody");
            messageBody.HowToBecomeData.Registrations[0].Id = registrationId.ToString();

            foreach (RouteEntry routeEntry in routeEntries)
            {
                messageBody.HowToBecomeData.RouteEntries.Add(routeEntry);
            }

            messageBody.JobProfileId = messageId.ToString();
            messageBody.UrlName = canonicalName;
            messageBody.CanonicalName = canonicalName;
            Message message = CreateServiceBusMessage(messageId, ConvertObjectToByteArray(messageBody), EnumLibrary.ContentType.JSON, ActionType.Published, CType.JobProfile);
            await topic.SendAsync(message);
        }

        public async Task UpdateMoreInformationLinksForRequirementType(Topic topic, UpdateMoreInformationLink updateMoreInformationLink, RequirementType requirementType)
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
                case RequirementType.University:
                    updateMessage.UserProperties.Add("CType", "UniversityLink");
                    break;

                case RequirementType.College:
                    updateMessage.UserProperties.Add("CType", "CollegeLink");
                    break;

                case RequirementType.Apprenticeship:
                    updateMessage.UserProperties.Add("CType", "ApprenticeshipLink");
                    break;
            }

            await topic.SendAsync(updateMessage);
        }

        public async Task UpdateEntryRequirementForRequirementType(Topic topic, EntryRequirementMessageBody updateEntryRequirement, RequirementType requirementType)
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
                case RequirementType.University:
                    updateMessage.UserProperties.Add("CType", "UniversityRequirement");
                    break;

                case RequirementType.College:
                    updateMessage.UserProperties.Add("CType", "CollegeRequirement");
                    break;

                case RequirementType.Apprenticeship:
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

        public UpdateRegistration GenerateRegistrationUpdate(Guid id, Guid jobProfileId, string info)
        {
            UpdateRegistration updateRegistration = ResourceManager.GetResource<UpdateRegistration>("UpdateRegistration");
            updateRegistration.Id = id.ToString();
            updateRegistration.JobProfileId = jobProfileId.ToString();
            updateRegistration.Info = info;
            return updateRegistration;
        }

        public UpdateMoreInformationLink GenerateMoreInformationLinkUpdate(string id, Guid jobProfileId, string linkText)
        {
            UpdateMoreInformationLink updateMoreInformationLink = ResourceManager.GetResource<UpdateMoreInformationLink>("UpdateMoreInformationLinks");
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

        public RouteEntry CreateARouteEntry(RequirementType requirementType)
        {
            string htmlId;
            switch (requirementType)
            {
                case RequirementType.University:
                    htmlId = "universityRouteSubjects";
                    break;

                case RequirementType.College:
                    htmlId = "collegeRouteSubjects";
                    break;

                case RequirementType.Apprenticeship:
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

        public Dictionary<RequirementType, HowToBecomeRouteEntry> GetRouteEntriesFromHtmlResponse(Response<HtmlDocument> response)
        {
            Dictionary<RequirementType, HowToBecomeRouteEntry> routeEntries = new Dictionary<RequirementType, HowToBecomeRouteEntry>();
            List<RequirementType> presentRequirementTypes = new List<RequirementType>();
            HtmlNode universitySection = response.Data.GetElementbyId("University");
            HtmlNode collegeSection = response.Data.GetElementbyId("College");
            HtmlNode apprenticeshipSection = response.Data.GetElementbyId("Apprenticeship");

            if (universitySection != null)
            {
                presentRequirementTypes.Add(RequirementType.University);
            }

            if (collegeSection != null)
            {
                presentRequirementTypes.Add(RequirementType.College);
            }

            if (apprenticeshipSection != null)
            {
                presentRequirementTypes.Add(RequirementType.Apprenticeship);
            }

            foreach (RequirementType requirementType in presentRequirementTypes)
            {
                string id = null;
                string xpathPrefix = null;

                switch (requirementType)
                {
                    case RequirementType.University:
                        id = "universityRouteSubjects";
                        xpathPrefix = "//section[@id='University']";
                        break;

                    case RequirementType.College:
                        id = "collegeRouteSubjects";
                        xpathPrefix = "//section[@id='College']";
                        break;

                    case RequirementType.Apprenticeship:
                        id = "apprenticeshipsRouteSubjects";
                        xpathPrefix = "//section[@id='Apprenticeship']";
                        break;
                }

                routeEntries.Add(requirementType, new HowToBecomeRouteEntry());
                routeEntries[requirementType].RouteSubjects = response.Data.DocumentNode.SelectSingleNode($"{xpathPrefix}//div[@id='{id}']").OuterHtml;
                routeEntries[requirementType].FurtherRouteInformation = response.Data.DocumentNode.SelectSingleNode($"{xpathPrefix}//p[@id='furtherRouteInformation']").OuterHtml;
                routeEntries[requirementType].RouteRequirement = response.Data.DocumentNode.SelectSingleNode($"{xpathPrefix}//p[2]").InnerText;
                HtmlNodeCollection entryRequirementsList = response.Data.DocumentNode.SelectNodes($"{xpathPrefix}//ul[@class='list-reqs']//li");
                routeEntries[requirementType].EntryRequirements = new List<EntryRequirement>();
                routeEntries[requirementType].EntryRequirements.Add(new EntryRequirement() { Info = entryRequirementsList[0].InnerText });
                HtmlNodeCollection moreInformationLinkList = response.Data.DocumentNode.SelectNodes($"{xpathPrefix}//ul[@class='list-link']//li");
                routeEntries[requirementType].MoreInformationLinks = new List<MoreInformationLink>();
                routeEntries[requirementType].MoreInformationLinks.Add(new MoreInformationLink() { Text = moreInformationLinkList[0].InnerText });
            }

            return routeEntries;
        }

        public UpdateRouteRequirement GenerateRouteRequirementUpdate(Guid id, string title)
        {
            UpdateRouteRequirement updateRouteRequirement = ResourceManager.GetResource<UpdateRouteRequirement>("UpdateRouteRequirement");
            updateRouteRequirement.Id = id.ToString();
            updateRouteRequirement.Title = title;
            return updateRouteRequirement;
        }
    }
}
