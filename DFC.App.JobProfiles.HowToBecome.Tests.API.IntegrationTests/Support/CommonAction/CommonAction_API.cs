using DFC.Api.JobProfiles.Common.APISupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.Interface;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support
{
    internal partial class CommonAction : IAPISupport
    {
        public async Task<Response<T>> ExecuteGetRequestWithJsonResponse<T>(string endpoint, bool AuthoriseRequest = true)
        {
            GetRequest getRequest = new GetRequest(endpoint, GetRequest.ContentType.Json);
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
            while (response.HttpStatusCode.Equals(HttpStatusCode.NoContent) && DateTime.Now - startTime < Settings.GracePeriod)
            {
                await Task.Delay(500);
                response = getRequest.Execute<T>();
            }

            return response;
        }

        public JobProfileContentType GenerateJobProfileContentType()
        {
            string canonicalName = RandomString(10);
            JobProfileContentType jobProfile = ResourceManager.GetResource<JobProfileContentType>("JobProfileCreateMessageBody");
            jobProfile.JobProfileId = Guid.NewGuid().ToString();
            jobProfile.UrlName = canonicalName;
            jobProfile.CanonicalName = canonicalName;
            return jobProfile;
        }

        public RouteEntry GenerateRouteEntryForRouteEntryType(EnumLibrary.RouteEntryType routeEntryType)
        {
            int? routeName = null;
            switch(routeEntryType)
            {
                case EnumLibrary.RouteEntryType.University:
                    routeName = 0;
                    break;

                case EnumLibrary.RouteEntryType.College:
                    routeName = 1;
                    break;

                case EnumLibrary.RouteEntryType.Apprenticeship:
                    routeName = 2;
                    break;
            }

            return new RouteEntry()
            {
                RouteName = (int)routeName,
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

        public async Task<Response<HtmlDocument>> ExecuteGetRequestWithHtmlResponse(string endpoint, bool AuthoriseRequest = true)
        {
            GetRequest getRequest = new GetRequest(endpoint, GetRequest.ContentType.Html);
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
    }
}