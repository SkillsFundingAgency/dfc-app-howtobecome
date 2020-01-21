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

        internal static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        internal static void InitialiseAppSettings()
        {
            IConfigurationRoot Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            Settings.ServiceBusConfig.Endpoint = Configuration.GetSection("ServiceBusConfig").GetSection("Endpoint").Value;
            Settings.APIConfig.Version = Configuration.GetSection("APIConfig").GetSection("Version").Value;
            Settings.APIConfig.ApimSubscriptionKey = Configuration.GetSection("APIConfig").GetSection("ApimSubscriptionKey").Value;
            Settings.APIConfig.EndpointBaseUrl.ProfileDetail = Configuration.GetSection("APIConfig").GetSection("EndpointBaseUrl").GetSection("ProfileDetail").Value;
            Settings.APIConfig.EndpointBaseUrl.HowToSegment = Configuration.GetSection("APIConfig").GetSection("EndpointBaseUrl").GetSection("HowToSegment").Value;
            if (!int.TryParse(Configuration.GetSection("GracePeriodInSeconds").Value, out int gracePeriodInSeconds)) { throw new InvalidCastException("Unable to retrieve an integer value for the grace period setting"); }
            Settings.GracePeriod = TimeSpan.FromSeconds(gracePeriodInSeconds);
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
            message.MessageId = Guid.NewGuid().ToString();
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

        internal async static Task DeleteJobProfileWithId(Topic topic, Guid jobProfileId)
        {
            JobProfileDeleteMessageBody messageBody = ResourceManager.GetResource<JobProfileDeleteMessageBody>("JobProfileDeleteMessageBody");
            messageBody.JobProfileId = jobProfileId.ToString();
            Message deleteMessage = CreateDeleteMessage(jobProfileId, ConvertObjectToByteArray(messageBody));
            await topic.SendAsync(deleteMessage);
        }

        internal async static Task CreateJobProfile(Topic topic, Guid messageId, string canonicalName, List<RouteEntry> routeEntries)
        {
            JobProfileCreateMessageBody messageBody = ResourceManager.GetResource<JobProfileCreateMessageBody>("JobProfileCreateMessageBody");
            
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

        internal async static Task<Response<T>> ExecuteGetRequestWithJsonResponse<T>(string endpoint, bool AuthoriseRequest = true)
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

        internal async static Task<Response<HtmlDocument>> ExecuteGetRequestWithHtmlResponse(string endpoint, bool AuthoriseRequest = true)
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

        public RouteEntry CreateARouteEntry(EnumLibrary.RequirementType requirementType)
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

                case RequirementType.Apprentiships:
                    htmlId = "apprentishipsRouteSubjects";
                    break;

                default:
                    throw new ArgumentException("Unrecognised requirement type");
            }

            RouteEntry routeEntry = ResourceManager.GetResource<RouteEntry>("HowToBecomeRouteEntry");
            routeEntry.RouteName = (int)requirementType;
            AddEntryRequirementToRouteEntry("Requirement one", routeEntry);
            AddEntryRequirementToRouteEntry("Requirement two", routeEntry);
            AddEntryRequirementToRouteEntry("Requirement three", routeEntry);
            AddMoreInformationLinkToRouteEntry("More information link one", routeEntry);
            AddMoreInformationLinkToRouteEntry("More information link two", routeEntry);
            AddMoreInformationLinkToRouteEntry("More information link three", routeEntry);
            routeEntry.RouteSubjects = $"<div id='{htmlId}'><p>This is a paragraph for route subjects.</p><ul><li>Listed item</li></ul></div>";
            routeEntry.FurtherRouteInformation = "<p id='furtherRouteInformation'>Automated further information</p>";
            routeEntry.RouteRequirement = "Automated requirement list";
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
                presentRequirementTypes.Add(RequirementType.Apprentiships);
            }

            foreach (RequirementType requirementType in presentRequirementTypes)
            {
                string id = null;

                switch (requirementType) {
                    case RequirementType.University:
                        id = "universityRouteSubjects";
                        break;

                    case RequirementType.College:
                        id = "collegeRouteSubjects";
                        break;

                    case RequirementType.Apprentiships:
                        id = "apprentishipsRouteSubjects";
                        break;
                }

                routeEntries.Add(requirementType, new HowToBecomeRouteEntry());
                routeEntries[requirementType].RouteSubjects = universitySection.SelectSingleNode($"//div[@id='{id}']").OuterHtml;
                routeEntries[requirementType].FurtherRouteInformation = universitySection.SelectSingleNode("//p[@id='furtherRouteInformation']").OuterHtml;
                routeEntries[requirementType].RouteRequirement = universitySection.SelectSingleNode("//p[2]").InnerText;
                HtmlNodeCollection entryRequirementsList = universitySection.SelectNodes("//ul[@class='list-reqs']//li");
                routeEntries[requirementType].EntryRequirements = new List<EntryRequirement>();
                routeEntries[requirementType].EntryRequirements.Add(new EntryRequirement() { Info = entryRequirementsList[0].InnerText });
                routeEntries[requirementType].EntryRequirements.Add(new EntryRequirement() { Info = entryRequirementsList[1].InnerText });
                routeEntries[requirementType].EntryRequirements.Add(new EntryRequirement() { Info = entryRequirementsList[2].InnerText });
                HtmlNodeCollection moreInformationLinkList = universitySection.SelectNodes("//ul[@class='list-link']//li");
                routeEntries[requirementType].MoreInformationLinks = new List<MoreInformationLink>();
                routeEntries[requirementType].MoreInformationLinks.Add(new MoreInformationLink() { Text = moreInformationLinkList[0].InnerText });
                routeEntries[requirementType].MoreInformationLinks.Add(new MoreInformationLink() { Text = moreInformationLinkList[1].InnerText });
                routeEntries[requirementType].MoreInformationLinks.Add(new MoreInformationLink() { Text = moreInformationLinkList[2].InnerText });
            }

            return routeEntries;
        }
    }
}
