using RestSharp;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.IntegrationTests.Support.API
{
    public interface IJobProfileApi
    {
        Task<IRestResponse<T>> GetById<T>(string id)
            where T : class, new();
    }
}