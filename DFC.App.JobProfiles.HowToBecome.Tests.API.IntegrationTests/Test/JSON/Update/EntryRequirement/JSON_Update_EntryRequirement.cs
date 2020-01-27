using DFC.Api.JobProfiles.Common.APISupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using HtmlAgilityPack;
using NUnit.Framework;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Test.JSON.Update.EntryRequirement
{
    public class JSON_Update_EntryRequirement : SetUpAndTearDown
    {
        [Test]
        public async Task Json_Update_EntryRequirement_University()
        {
            string newEntryRequirementText = "new entry requirement text for university";
            EntryRequirementMessageBody EntryRequirementMessageBody = CommonAction.CreateEntryRequirementMessageBody(UniversityRouteEntry.EntryRequirements[0].Id, JobProfileId, newEntryRequirementText);
            await CommonAction.UpdateEntryRequirementForRequirementType(Topic, EntryRequirementMessageBody, RequirementType.University);
            await Task.Delay(5000);
            Response<HtmlDocument> howToBecomeResponse = await CommonAction.ExecuteGetRequestWithJsonResponse<HtmlDocument>(Settings.APIConfig.EndpointBaseUrl.JSONContent.Replace("{id}", JobProfileId.ToString()));
            //TODO
        }

        [Test]
        public async Task Json_Update_EntryRequirement_College()
        {
            string newEntryRequirementText = "new entry requirement text for college";
            CommonAction commonAction = new CommonAction();
            EntryRequirementMessageBody updateEntryRequirement = commonAction.CreateEntryRequirementMessageBody(CollegeRouteEntry.EntryRequirements[0].Id, JobProfileId, newEntryRequirementText);
            await commonAction.UpdateEntryRequirementForRequirementType(Topic, updateEntryRequirement, RequirementType.College);
            await Task.Delay(5000);
            Response<HtmlDocument> howToBecomeResponse = await CommonAction.ExecuteGetRequestWithJsonResponse<HtmlDocument>(Settings.APIConfig.EndpointBaseUrl.JSONContent.Replace("{id}", JobProfileId.ToString()));
            //TODO
        }

        [Test]
        public async Task Json_Update_EntryRequirement_Apprenticeship()
        {
            string newEntryRequirementText = "new entry requirement text for apprenticeship";
            CommonAction commonAction = new CommonAction();
            EntryRequirementMessageBody updateEntryRequirement = commonAction.CreateEntryRequirementMessageBody(ApprenticeshipRouteEntry.EntryRequirements[0].Id, JobProfileId, newEntryRequirementText);
            await commonAction.UpdateEntryRequirementForRequirementType(Topic, updateEntryRequirement, RequirementType.Apprenticeship);
            await Task.Delay(5000);
            Response<HtmlDocument> howToBecomeResponse = await CommonAction.ExecuteGetRequestWithJsonResponse<HtmlDocument>(Settings.APIConfig.EndpointBaseUrl.JSONContent.Replace("{id}", JobProfileId.ToString()));
            //TODO
        }
    }
}