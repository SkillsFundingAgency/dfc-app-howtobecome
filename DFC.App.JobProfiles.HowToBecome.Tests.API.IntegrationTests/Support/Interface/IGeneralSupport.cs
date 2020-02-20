using System;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.Interface
{
    public interface IGeneralSupport
    {
        string GenerateUpperCaseRandomAlphaString(int length);

        void InitialiseAppSettings();

        byte[] ConvertObjectToByteArray(object obj);

        string GetDescription(Enum enumerator);
    }
}
