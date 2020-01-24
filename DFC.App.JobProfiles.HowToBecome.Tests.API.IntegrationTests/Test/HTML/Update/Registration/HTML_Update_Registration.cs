using DFC.Api.JobProfiles.Common.APISupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using HtmlAgilityPack;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Test.HTML.Update.Registration
{
    public class HTML_Update_Registration : SetUpAndTearDown
    {
        [Test]
        public async Task Html_Registration_Apprenticeship()
        {
            string newRegistrationValue = "<p>new registration text for apprenticeship</p>";
            CommonAction commonAction = new CommonAction();
            UpdateRegistration updateRegistration = commonAction.GenerateRegistrationUpdate(RegistrationId, JobProfileId, newRegistrationValue);
            await commonAction.UpdateRegistration(Topic, updateRegistration);
            await Task.Delay(5000);
            Response<HtmlDocument> howToBecomeResponse = await commonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl.HowToSegment + CanonicalName);
            string observedRegistrationValue = howToBecomeResponse.Data.DocumentNode.SelectSingleNode("//section[@id='moreinfo']/div[@class='job-profile-subsection-content']/ul/li").InnerHtml;
            Assert.AreEqual(newRegistrationValue, observedRegistrationValue);
        }
    }
}