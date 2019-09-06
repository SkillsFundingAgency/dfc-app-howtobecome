using RazorEngine.Templating;

namespace DFC.App.JobProfiles.HowToBecome.Views.Tests.ViewRenderer
{
    public class HtmlSupportTemplateBase<T> : TemplateBase<T>
    {
        public HtmlSupportTemplateBase()
        {
            Html = new RazorHtmlHelper();
        }

        public RazorHtmlHelper Html { get; set; }

        public void IgnoreSection(string sectionName)
        {
        }
    }
}