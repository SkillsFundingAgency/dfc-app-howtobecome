using RazorEngine.Text;

namespace DFC.App.JobProfiles.HowToBecome.Views.Tests.ViewRenderer
{
    public class RazorHtmlHelper
    {
        public IEncodedString Raw(string rawString)
        {
            return new RawString(rawString);
        }
    }
}