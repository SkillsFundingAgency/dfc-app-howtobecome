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
    public partial class CommonAction
    {
        private static Random random = new Random();

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
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

        public T GetResource<T>(string resourceName)
        {
            var resourcesDirectory = Directory.CreateDirectory(Environment.CurrentDirectory).GetDirectories("Resource")[0];
            var files = resourcesDirectory.GetFiles();
            var selectedResource = files.FirstOrDefault(file => file.Name.ToUpperInvariant().StartsWith(resourceName.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase));
            if (selectedResource == null)
            {
                throw new Exception($"No resource with the name {resourceName} was found");
            }

            using var streamReader = new StreamReader(selectedResource.FullName);
            var content = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
