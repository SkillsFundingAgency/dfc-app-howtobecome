using DFC.Api.JobProfiles.Common.APISupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.Interface
{
    internal interface IHowToBecomeSupport
    {
        void AddEntryRequirementToRouteEntry(string entryRequirementInformation, RouteEntry routeEntry);
        void AddMoreInformationLinkToRouteEntry(string linkText, RouteEntry routeEntry);
        RouteEntry CreateARouteEntry(RequirementType requirementType);
        RouteEntry UpdateRouteEntryWithPrefix(RouteEntry routeEntry, string prefix);
        Dictionary<RequirementType, HowToBecomeRouteEntry> GetRouteEntriesFromHtmlResponse(Response<HtmlDocument> response);
        UpdateRouteRequirement GenerateRouteRequirementUpdate(Guid id, string title);
    }
}
