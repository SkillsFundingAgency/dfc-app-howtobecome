using DFC.Api.JobProfiles.Common.APISupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using HtmlAgilityPack;
using System.Collections.Generic;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.Interface
{
    internal interface IHowToBecomeSupport
    {
        void AddEntryRequirementToRouteEntry(string entryRequirementInformation, RouteEntry routeEntry);
        void AddMoreInformationLinkToRouteEntry(string linkText, RouteEntry routeEntry);
        RouteEntry CreateARouteEntry(RequirementType requirementType);
        Dictionary<RequirementType, HowToBecomeRouteEntry> GetRouteEntriesFromHtmlResponse(Response<HtmlDocument> response);
    }
}
