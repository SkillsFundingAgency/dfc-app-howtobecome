namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.CommonActions.Interface
{
    public interface IGeneralSupport
    {
        string RandomString(int length);

        byte[] ConvertObjectToByteArray(object obj);

        T GetResource<T>(string resourceName);
    }
}
