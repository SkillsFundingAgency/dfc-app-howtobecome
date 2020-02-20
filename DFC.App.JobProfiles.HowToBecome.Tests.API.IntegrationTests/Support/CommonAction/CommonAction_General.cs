using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.Interface;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support
{
    public partial class CommonAction : IGeneralSupport
    {
        private static Random random = new Random();

        public string GenerateUpperCaseRandomAlphaString(int stringLength)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, stringLength)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void InitialiseAppSettings()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            Settings.ServiceBusConfig.Endpoint = configuration.GetSection("ServiceBusConfig").GetSection("Endpoint").Value;
            Settings.APIConfig.Version = configuration.GetSection("APIConfig").GetSection("Version").Value;
            Settings.APIConfig.ApimSubscriptionKey = configuration.GetSection("APIConfig").GetSection("ApimSubscriptionKey").Value;
            Settings.APIConfig.EndpointBaseUrl = configuration.GetSection("APIConfig").GetSection("EndpointBaseUrl").Value;
            if (!int.TryParse(configuration.GetSection("GracePeriodInSeconds").Value, out int gracePeriodInSeconds))
            {
                throw new InvalidCastException("Unable to retrieve an integer value for the grace period setting");
            }

            Settings.GracePeriod = TimeSpan.FromSeconds(gracePeriodInSeconds);
        }

        public byte[] ConvertObjectToByteArray(object obj)
        {
            string serialisedContent = JsonConvert.SerializeObject(obj);
            return Encoding.ASCII.GetBytes(serialisedContent);
        }

        public string GetDescription(Enum enumerator)
        {
            Type type = enumerator.GetType();
            MemberInfo[] memInfo = type.GetMember(enumerator.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = (object[])memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return enumerator.ToString();
        }
    }
}
