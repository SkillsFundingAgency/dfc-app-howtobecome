using DFC.Api.JobProfiles.Common.APISupport;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Support.Interface
{
    public interface IAPISupport
    {
        Task<Response<T>> ExecuteGetRequest<T>(string endpoint, bool authoriseRequest = true);
    }
}
