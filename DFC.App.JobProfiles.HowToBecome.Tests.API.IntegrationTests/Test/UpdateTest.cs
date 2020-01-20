using DFC.Api.JobProfiles.Common.APISupport;
using DFC.Api.JobProfiles.IntegrationTests.Support;
using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model;
using HtmlAgilityPack;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests
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
            HowToBecomeRouteEntry input = ResourceManager.GetResource<HowToBecomeRouteEntry>("HowToBecomeRouteEntry");
            HtmlDocument routeSubjects = new HtmlDocument();
            string inputWithoutNewLines = input.RouteSubjects.Replace(Environment.NewLine, string.Empty).Replace('\u000a'.ToString(), string.Empty).Trim();
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