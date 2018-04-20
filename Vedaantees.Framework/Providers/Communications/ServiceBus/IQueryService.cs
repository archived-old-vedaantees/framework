using Vedaantees.Framework.Types.Results;

namespace Vedaantees.Framework.Providers.Communications.ServiceBus
{
    public interface IQueryService
    {
        MethodResult<TResponse> ExecuteQuery<TRequest, TResponse>(TRequest request)
            where TRequest : QueryRequest<TResponse>;
    }
}