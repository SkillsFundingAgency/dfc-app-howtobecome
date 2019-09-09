using Microsoft.Extensions.Configuration;

namespace DFC.App.JobProfiles.HowToBecome.Views.Tests.Tests
{
    public class TestsBase
    {
        protected string viewRootPath;

        public TestsBase()
        {
            var config = new ConfigurationBuilder();
            config.AddJsonFile("appsettings.json");
            var configuration = config.Build();

            viewRootPath = configuration["ViewRootPath"];
        }
    }
}