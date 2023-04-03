namespace DFC.App.JobProfiles.HowToBecome.FunctionalTests.Support.CommonActions.Interface
{
    public interface IGeneralSupport
    {
        string RandomString(int length);

        byte[] ConvertObjectToByteArray(object obj);

        T GetResource<T>(string resourceName);
    }
}
