﻿using DFC.Api.JobProfiles.Common.APISupport;
using DFC.Api.JobProfiles.Common.AzureServiceBusSupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.Interface
{
    internal interface IHowToBecomeSupport
    {
        void AddEntryRequirementToRouteEntry(string entryRequirementInformation, RouteEntry routeEntry);
        void AddMoreInformationLinkToRouteEntry(string linkText, RouteEntry routeEntry);
        RouteEntry CreateARouteEntry(RouteEntryType requirementType);
        Dictionary<RouteEntryType, HowToBecomeRouteEntry> GetRouteEntriesFromHtmlResponse(Response<HtmlDocument> response);
        UpdateRouteRequirement GenerateRouteRequirementContentTypeForJobProfile(Guid id, string title);
        Task UpdateRouteRequirement(Topic topic, UpdateRouteRequirement updateRouteRequirement, RouteEntryType requirementType);
        Task UpdateRegistration(Topic topic, UpdateRegistration updateRegistration);
        UpdateRegistration GenerateRegistrationUpdate(Guid id, Guid jobProfileId, string info);
        Task DeleteJobProfileWithId(Topic topic, Guid jobProfileId);
        Task<JobProfileCreateMessageBody> CreateJobProfile(Topic topic);
        UpdateMoreInformationLink GenerateMoreInformationLinkUpdate(string id, Guid jobProfileId, string linkText);
        Task UpdateMoreInformationLinksForRequirementType(Topic topic, UpdateMoreInformationLink updateMoreInformationLink, RouteEntryType requirementType);
        Task UpdateEntryRequirementForRequirementType(Topic topic, EntryRequirementMessageBody updateEntryRequirement, RouteEntryType requirementType);
        EntryRequirementMessageBody CreateEntryRequirementMessageBody(string entryRequirementId, Guid jobProfileId, string entryRequirementInfo);
        EntryRequirement CreateEntryRequirement(string requirementInformation);
        MoreInformationLink CreateMoreInformationLink(string linkText);
    }
}
