using DFC.Api.JobProfiles.Common.APISupport;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using HtmlAgilityPack;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.EnumLibrary;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Test
{
    public class Tests : Hook
    {
        [Test]
        public async Task Json()
        {
            Response<HowToBecomeRouteEntry> howToBecomeRouteEntry = await CommonAction.ExecuteGetRequestWithJsonResponse<HowToBecomeRouteEntry>(Settings.APIConfig.EndpointBaseUrl + CanonicalName);
        }

        [Test]
        public async Task Html()
        {
            CommonAction commonAction = new CommonAction();
            JobProfileCreateMessageBody createInput = ResourceManager.GetResource<JobProfileCreateMessageBody>("JobProfileCreateMessageBody");
            
            RouteEntry universityRouteEntry = ResourceManager.GetResource<RouteEntry>("HowToBecomeRouteEntry");
            universityRouteEntry.RouteName = (int)RequirementType.University;
            commonAction.AddEntryRequirementToRouteEntry("Requirement one", universityRouteEntry);
            commonAction.AddEntryRequirementToRouteEntry("Requirement two", universityRouteEntry);
            commonAction.AddEntryRequirementToRouteEntry("Requirement three", universityRouteEntry);
            commonAction.AddMoreInformationLinkToRouteEntry("More information link one", universityRouteEntry);
            commonAction.AddMoreInformationLinkToRouteEntry("More information link two", universityRouteEntry);
            commonAction.AddMoreInformationLinkToRouteEntry("More information link three", universityRouteEntry);
            universityRouteEntry.RouteSubjects = "<div id='universityRouteSubjects'><p>This is a paragraph for route subjects.</p><ul><li>Listed item</li></ul></div>";
            universityRouteEntry.FurtherRouteInformation = "Automated further information";
            universityRouteEntry.RouteRequirement = "Automated requirement list";
            createInput.HowToBecomeData.RouteEntries.Add(universityRouteEntry);

            RouteEntry collegeRouteEntry = ResourceManager.GetResource<RouteEntry>("HowToBecomeRouteEntry");
            universityRouteEntry.RouteName = (int)RequirementType.College;
            commonAction.AddEntryRequirementToRouteEntry("Requirement one", collegeRouteEntry);
            commonAction.AddEntryRequirementToRouteEntry("Requirement two", collegeRouteEntry);
            commonAction.AddEntryRequirementToRouteEntry("Requirement three", collegeRouteEntry);
            commonAction.AddMoreInformationLinkToRouteEntry("More information link one", collegeRouteEntry);
            commonAction.AddMoreInformationLinkToRouteEntry("More information link two", collegeRouteEntry);
            commonAction.AddMoreInformationLinkToRouteEntry("More information link three", collegeRouteEntry);
            universityRouteEntry.RouteSubjects = "<div id='collegeRouteSubjects'><p>This is a paragraph for route subjects.</p><ul><li>Listed item</li></ul></div>";
            universityRouteEntry.FurtherRouteInformation = "Automated further information";
            universityRouteEntry.RouteRequirement = "Automated requirement list";
            createInput.HowToBecomeData.RouteEntries.Add(collegeRouteEntry);

            RouteEntry apprentishipRouteEntry = ResourceManager.GetResource<RouteEntry>("HowToBecomeRouteEntry");
            universityRouteEntry.RouteName = (int)RequirementType.Apprentiships;
            commonAction.AddEntryRequirementToRouteEntry("Requirement one", apprentishipRouteEntry);
            commonAction.AddEntryRequirementToRouteEntry("Requirement two", apprentishipRouteEntry);
            commonAction.AddEntryRequirementToRouteEntry("Requirement three", apprentishipRouteEntry);
            commonAction.AddMoreInformationLinkToRouteEntry("More information link one", apprentishipRouteEntry);
            commonAction.AddMoreInformationLinkToRouteEntry("More information link two", apprentishipRouteEntry);
            commonAction.AddMoreInformationLinkToRouteEntry("More information link three", apprentishipRouteEntry);
            universityRouteEntry.RouteSubjects = "<div id='apprentishipRouteSubjects'><p>This is a paragraph for route subjects.</p><ul><li>Listed item</li></ul></div>";
            universityRouteEntry.FurtherRouteInformation = "Automated further information";
            universityRouteEntry.RouteRequirement = "Automated requirement list";
            createInput.HowToBecomeData.RouteEntries.Add(apprentishipRouteEntry);

            Response<JobProfileCreateMessageBody> jobProfileCreateMessageBody = await CommonAction.ExecuteGetRequestWithJsonResponse<JobProfileCreateMessageBody>(Settings.APIConfig.EndpointBaseUrl);



            HtmlDocument routeSubjects = new HtmlDocument();
            string inputWithoutNewLines = universityRouteEntry.RouteSubjects.Replace(Environment.NewLine, string.Empty).Replace('\u000a'.ToString(), string.Empty).Trim();
            routeSubjects.LoadHtml(inputWithoutNewLines);

            List<HtmlNode> childNodes = new List<HtmlNode>();
            foreach (HtmlNode childNode in routeSubjects.DocumentNode.ChildNodes)
            {
                if (!childNode.OuterHtml.Equals(string.Empty))
                {
                    childNodes.Add(childNode);
                }
            }

            Response<HtmlDocument> howToBecomeRouteEntry = await CommonAction.ExecuteGetRequestWithHtmlResponse(Settings.APIConfig.EndpointBaseUrl + CanonicalName);
            HtmlNode universitySection = howToBecomeRouteEntry.Data.GetElementbyId("University");
            string html = universitySection.SelectNodes("//div[@class='job-profile-subsection-content']")[0].InnerHtml;
            html = html.Replace(Environment.NewLine, string.Empty).Replace('\u000a'.ToString(), string.Empty).Trim();

            HtmlDocument output = new HtmlDocument();
            output.LoadHtml(html);

            List<HtmlNode> outputChildNodes = new List<HtmlNode>();
            for (int nodeIndex = 0; nodeIndex < childNodes.Count; nodeIndex++)
            {
                outputChildNodes.Add(output.DocumentNode.ChildNodes[nodeIndex]);
            }

            bool isContentTheSame = true;
            for (int nodeIndex = 0; nodeIndex < childNodes.Count; nodeIndex++)
            {
                if (!childNodes[nodeIndex].InnerHtml.Equals(outputChildNodes[nodeIndex].InnerHtml))
                {
                    isContentTheSame = false;
                }
            }

            Assert.IsTrue(isContentTheSame);
        }
    }
}