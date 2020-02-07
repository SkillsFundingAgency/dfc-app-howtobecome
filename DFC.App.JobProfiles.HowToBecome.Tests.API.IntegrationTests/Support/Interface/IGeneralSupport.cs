using System;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.Interface
{
    internal interface IGeneralSupport
    {
        string RandomString(int length);

        void InitialiseAppSettings();

        byte[] ConvertObjectToByteArray(object obj);

        string GetDescription(Enum enumerator);
    }
}
