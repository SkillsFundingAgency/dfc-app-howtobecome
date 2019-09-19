using Microsoft.AspNetCore.Html;
using RazorEngine.Text;

namespace DFC.App.JobProfiles.HowToBecome.Views.Tests.ViewRenderer
{
    public class RazorHtmlHelper
    {
        public IEncodedString Raw(HtmlString rawString)
        {
            return new RawString(rawString?.Value);
        }

        public IEncodedString Raw(string rawString)
        {
            return new RawString(rawString);
        }
    }
}