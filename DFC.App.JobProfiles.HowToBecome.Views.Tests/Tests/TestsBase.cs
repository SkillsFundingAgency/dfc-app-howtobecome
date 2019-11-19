using Microsoft.Extensions.Configuration;

namespace DFC.App.JobProfiles.HowToBecome.Views.Tests.Tests
{
    public class TestsBase
    {
        public TestsBase()
        {
            var config = new ConfigurationBuilder();
            config.AddJsonFile("appsettings.json");
            var configuration = config.Build();

            ViewRootPath = configuration["ViewRootPath"];
        }

        protected string ViewRootPath { get; }
    }
}