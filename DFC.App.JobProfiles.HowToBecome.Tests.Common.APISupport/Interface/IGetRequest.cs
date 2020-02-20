using DFC.Api.JobProfiles.Common.APISupport;
using static DFC.Api.JobProfiles.Common.APISupport.GetRequest;

namespace DFC.App.JobProfiles.HowToBecome.Tests.Common.APISupport.Interface
{
    public interface IGetRequest
    {
        void AddVersionHeader(string version);

        void AddApimKeyHeader(string apimSubscriptionKey);

        void AddAcceptHeader(AcceptType contentType);

        Response<T> Execute<T>();
    }
}
