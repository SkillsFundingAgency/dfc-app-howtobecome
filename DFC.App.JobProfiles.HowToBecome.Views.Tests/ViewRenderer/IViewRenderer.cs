using System.Collections.Generic;

namespace DFC.App.JobProfiles.HowToBecome.Views.Tests.ViewRenderer
{
    public interface IViewRenderer
    {
        string Render(string viewName, object model, IDictionary<string, object> viewBag);
    }
}