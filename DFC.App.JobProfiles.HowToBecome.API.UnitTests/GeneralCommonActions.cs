using DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support;
using FakeItEasy;
using NUnit.Framework;
using System.Linq;

namespace DFC.App.JobProfiles.HowToBecome.API.UnitTests
{
    public class GeneralCommonActions
    {
        [Test]
        public void RandomStringLengthOfString10()
        {
            Assert.AreEqual(10, new CommonAction().GenerateUpperCaseRandomAlphaString(10).Length);
        }

        [Test]
        public void RandomStringLengthOfString0()
        {
            Assert.AreEqual(0, new CommonAction().GenerateUpperCaseRandomAlphaString(0).Length);
        }

        [Test]
        public void RandomStringAlphaCharactersOnly()
        {
            var numberOfIntegersInRandomAlphaString = (from t in new CommonAction().GenerateUpperCaseRandomAlphaString(1000)
                             where char.IsDigit(t)
                             select t).ToArray().Length;

            Assert.AreEqual(0, numberOfIntegersInRandomAlphaString);
        }

        [Test]
        public void RandomStringUppercase()
        {
            var numberOfLowerCaseCharactersInRandomAlphaString = (from t in new CommonAction().GenerateUpperCaseRandomAlphaString(1000)
                                                       where char.IsLower(t)
                                                       select t).ToArray().Length;

            Assert.AreEqual(0, numberOfLowerCaseCharactersInRandomAlphaString);
        }
    }
}