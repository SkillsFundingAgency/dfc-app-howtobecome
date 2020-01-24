using DFC.Api.JobProfiles.Common.APISupport;
using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.Interface;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static DFC.Api.JobProfiles.Common.APISupport.GetRequest;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support
{
    internal class CommonAction : IHowToBecomeSupport
    {
        private static Random Random = new Random();

        internal string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        internal void InitialiseAppSettings()
        {
            IConfigurationRoot Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            Settings.ServiceBusConfig.Endpoint = Configuration.GetSection("ServiceBusConfig").GetSection("Endpoint").Value;
            Settings.APIConfig.Version = Configuration.GetSection("APIConfig").GetSection("Version").Value;
            Settings.APIConfig.ApimSubscriptionKey = Configuration.GetSection("APIConfig").GetSection("ApimSubscriptionKey").Value;
            Settings.APIConfig.EndpointBaseUrl.ProfileDetail = Configuration.GetSection("APIConfig").GetSection("EndpointBaseUrl").GetSection("ProfileDetail").Value;
            Settings.APIConfig.EndpointBaseUrl.HowToSegment = Configuration.GetSection("APIConfig").GetSection("EndpointBaseUrl").GetSection("HowToSegment").Value;
            if (!int.TryParse(Configuration.GetSection("GracePeriodInSeconds").Value, out int gracePeriodInSeconds)) { throw new InvalidCastException("Unable to retrieve an integer value for the grace period setting"); }
            Settings.GracePeriod = TimeSpan.FromSeconds(gracePeriodInSeconds);
            Settings.UpdatedRecordPrefix = Configuration.GetSection("UpdatedRecordPrefix").Value;
        }

        internal async Task UpdateRouteRequirement(Topic topic, UpdateRouteRequirement updateRouteRequirement, RequirementType requirementType)
        {
            Message updateMessage = new Message();
            updateMessage.ContentType = "application/json";
            updateMessage.Body = ConvertObjectToByteArray(updateRouteRequirement);
            updateMessage.CorrelationId = Guid.NewGuid().ToString();
            updateMessage.Label = "Automated route requirement message";
            updateMessage.MessageId = updateRouteRequirement.Id;
            updateMessage.UserProperties.Add("Id", updateRouteRequirement.JobProfileId);
            updateMessage.UserProperties.Add("ActionType", "Published");

            switch(requirementType)
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

        internal async Task UpdateRegistration(Topic topic, UpdateRegistration updateRegistration)
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

        internal UpdateRegistration GenerateRegistrationUpdate(Guid id, Guid jobProfileId, string info)
        {
            UpdateRegistration updateRegistration = ResourceManager.GetResource<UpdateRegistration>("UpdateRegistration");
            updateRegistration.Id = id.ToString();
            updateRegistration.JobProfileId = jobProfileId.ToString();
            updateRegistration.Info = info;
            return updateRegistration;
        }

        private static byte[] ConvertObjectToByteArray(object obj)
        {
            string serialisedContent = JsonConvert.SerializeObject(obj);
            return Encoding.ASCII.GetBytes(serialisedContent);
        }

        private static Message CreateCreateMessage(Guid messageId, byte[] messageBody)
        {
            Message message = new Message();
            message.ContentType = "application/json";
            message.Body = messageBody;
            message.CorrelationId = Guid.NewGuid().ToString();
            message.Label = "Automated message";
            message.MessageId = messageId.ToString();
            message.UserProperties.Add("Id", messageId);
            message.UserProperties.Add("ActionType", "Published");
            message.UserProperties.Add("CType", "JobProfile");
            return message;
        }

        private static Message CreateDeleteMessage(Guid messageId, byte[] messageBody)
        {
            Message message = new Message();
            message.ContentType = "application/json";
            message.UserProperties.Add("Id", messageId);
            message.UserProperties.Add("ActionType", "Deleted");
            message.UserProperties.Add("CType", "JobProfile");
            message.Label = "Automated message";
            message.Body = messageBody;
            return message;
        }

        internal async static Task UpdateJobProfileWithId(Topic topic, Guid messageId, string canonicalName, RouteEntry[] routeEntries)
        {
            JobProfileCreateMessageBody messageBody = ResourceManager.GetResource<JobProfileCreateMessageBody>("JobProfileCreateMessageBody");

            foreach (RouteEntry routeEntry in routeEntries)
            {
                messageBody.HowToBecomeData.RouteEntries.Add(routeEntry);
            }

            messageBody.JobProfileId = messageId.ToString();
            messageBody.UrlName = canonicalName;
            messageBody.CanonicalName = canonicalName;
            Message message = CreateCreateMessage(messageId, ConvertObjectToByteArray(messageBody));
            await topic.SendAsync(message);
        }

        internal async static Task DeleteJobProfileWithId(Topic topic, Guid jobProfileId)
        {
            JobProfileDeleteMessageBody messageBody = ResourceManager.GetResource<JobProfileDeleteMessageBody>("JobProfileDeleteMessageBody");
            messageBody.JobProfileId = jobProfileId.ToString();
            Message deleteMessage = CreateDeleteMessage(jobProfileId, ConvertObjectToByteArray(messageBody));
            await topic.SendAsync(deleteMessage);
        }

        internal async static Task CreateJobProfile(Topic topic, Guid messageId, Guid registrationId, string canonicalName, List<RouteEntry> routeEntries)
        {
            JobProfileCreateMessageBody messageBody = ResourceManager.GetResource<JobProfileCreateMessageBody>("JobProfileCreateMessageBody");
            messageBody.HowToBecomeData.Registrations[0].Id = registrationId.ToString();

            foreach(RouteEntry routeEntry in routeEntries)
            {
                messageBody.HowToBecomeData.RouteEntries.Add(routeEntry);
            }
            
            messageBody.JobProfileId = messageId.ToString();
            messageBody.UrlName = canonicalName;
            messageBody.CanonicalName = canonicalName;
            Message message = CreateCreateMessage(messageId, ConvertObjectToByteArray(messageBody));
            await topic.SendAsync(message);
        }

        internal async Task<Response<T>> ExecuteGetRequestWithJsonResponse<T>(string endpoint, bool AuthoriseRequest = true)
        {
            GetRequest getRequest = new GetRequest(endpoint, ContentType.Json);
            getRequest.AddVersionHeader(Settings.APIConfig.Version);

            if (AuthoriseRequest)
            {
                getRequest.AddApimKeyHeader(Settings.APIConfig.ApimSubscriptionKey);
            }
            else
            {
                getRequest.AddApimKeyHeader(RandomString(20).ToLower());
            }

            Response<T> response = getRequest.Execute<T>();
            DateTime startTime = DateTime.Now;
            while(response.HttpStatusCode.Equals(HttpStatusCode.NoContent) && DateTime.Now - startTime < Settings.GracePeriod)
            {
                await Task.Delay(500);
                response = getRequest.Execute<T>();
            }

            return response;
        }

        internal UpdateMoreInformationLink GenerateMoreInformationLinkUpdate(string id, Guid jobProfileId, string linkText)
        {
            UpdateMoreInformationLink updateMoreInformationLink = ResourceManager.GetResource<UpdateMoreInformationLink>("UpdateMoreInformationLinks");
            updateMoreInformationLink.Id = id;
            updateMoreInformationLink.JobProfileId = jobProfileId.ToString();
            updateMoreInformationLink.Text = linkText;
            return updateMoreInformationLink;
        }

        internal async Task UpdateMoreInformationLinks(Topic topic, UpdateMoreInformationLink updateMoreInformationLink, RequirementType requirementType)
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

        internal async Task<Response<HtmlDocument>> ExecuteGetRequestWithHtmlResponse(string endpoint, bool AuthoriseRequest = true)
        {
            GetRequest getRequest = new GetRequest(endpoint, ContentType.Html);
            getRequest.AddVersionHeader(Settings.APIConfig.Version);

            if (AuthoriseRequest)
            {
                getRequest.AddApimKeyHeader(Settings.APIConfig.ApimSubscriptionKey);
            }
            else
            {
                getRequest.AddApimKeyHeader(RandomString(20).ToLower());
            }

            Response<HtmlDocument> response = getRequest.Execute();
            DateTime startTime = DateTime.Now;
            while (response.HttpStatusCode.Equals(HttpStatusCode.NoContent) && DateTime.Now - startTime < Settings.GracePeriod)
            {
                await Task.Delay(500);
                response = getRequest.Execute();
            }

            return response;
        }

        public void AddEntryRequirementToRouteEntry(string entryRequirementInformation, RouteEntry routeEntry)
        {
            EntryRequirement entryRequirement = CreateEntryRequirement(entryRequirementInformation);
            routeEntry.EntryRequirements.Add(entryRequirement);
        }

        internal async Task UpdateEntryRequirement(Topic topic, UpdateEntryRequirement updateEntryRequirement, RequirementType requirementType)
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

        internal UpdateEntryRequirement GenerateEntryRequirementUpdate(string id, Guid jobProfileId, string info)
        {
            UpdateEntryRequirement updateEntryRequirement = ResourceManager.GetResource<UpdateEntryRequirement>("UpdateEntryRequirement");
            updateEntryRequirement.Id = id.ToString();
            updateEntryRequirement.JobProfileId = jobProfileId.ToString();
            updateEntryRequirement.Info = info;
            return updateEntryRequirement;
        }

        private EntryRequirement CreateEntryRequirement(string requirementInformation)
        {
            EntryRequirement entryRequirement = new EntryRequirement();
            entryRequirement.Id = Guid.NewGuid().ToString();
            entryRequirement.Title = "New entry requirement";
            entryRequirement.Info = requirementInformation;
            return entryRequirement;
        }

        public void AddMoreInformationLinkToRouteEntry(string linkText, RouteEntry routeEntry)
        {
            MoreInformationLink moreInformationLink = CreateMoreInformationLink(linkText);
            routeEntry.MoreInformationLinks.Add(moreInformationLink);
        }

        private MoreInformationLink CreateMoreInformationLink(string linkText)
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
            switch(requirementType)
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

            if(universitySection != null)
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

                switch (requirementType) {
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

        public RouteEntry UpdateRouteEntryWithPrefix(RouteEntry routeEntry, string prefix)
        {
            routeEntry.RouteSubjects = UpdateRouteSubjectsStringWithPrefix(routeEntry.RouteSubjects, prefix);
            routeEntry.FurtherRouteInformation = UpdateFurtherInformationStringWithPrefix(routeEntry.FurtherRouteInformation, prefix);
            routeEntry.RouteRequirement = $"{prefix} {routeEntry.RouteRequirement}";

            foreach(EntryRequirement entryRequirement in routeEntry.EntryRequirements)
            {
                entryRequirement.Info = $"{prefix} {entryRequirement.Info}";
            }

            foreach(MoreInformationLink moreInformtionLink in routeEntry.MoreInformationLinks)
            {
                moreInformtionLink.Text = $"{prefix} {moreInformtionLink.Text}";
            }

            return routeEntry;
        }

        private string UpdateRouteSubjectsStringWithPrefix(string routeSubjects, string prefix)
        {
            return $"{routeSubjects.Split("<p>")[0]} <p>{prefix} {routeSubjects.Split("<p>")[1]}";
        }

        private string UpdateFurtherInformationStringWithPrefix(string routeSubjects, string prefix)
        {
            return $"{routeSubjects.Split("'>")[0]}'>{prefix} {routeSubjects.Split("'>")[1]}";
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
