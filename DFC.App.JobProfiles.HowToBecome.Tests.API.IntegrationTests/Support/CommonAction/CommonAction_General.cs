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
    internal partial class CommonAction : IGeneralSupport
    {
        private static Random Random = new Random();

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public void InitialiseAppSettings()
        {
            IConfigurationRoot Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            Settings.ServiceBusConfig.Endpoint = Configuration.GetSection("ServiceBusConfig").GetSection("Endpoint").Value;
            Settings.APIConfig.Version = Configuration.GetSection("APIConfig").GetSection("Version").Value;
            Settings.APIConfig.ApimSubscriptionKey = Configuration.GetSection("APIConfig").GetSection("ApimSubscriptionKey").Value;
            Settings.APIConfig.EndpointBaseUrl = Configuration.GetSection("APIConfig").GetSection("EndpointBaseUrl").Value;
            if (!int.TryParse(Configuration.GetSection("GracePeriodInSeconds").Value, out int gracePeriodInSeconds)) { throw new InvalidCastException("Unable to retrieve an integer value for the grace period setting"); }
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
