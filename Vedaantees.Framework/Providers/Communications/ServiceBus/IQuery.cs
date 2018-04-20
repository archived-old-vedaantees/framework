namespace Vedaantees.Framework.Providers.Communications.ServiceBus
{
    public interface IQuery<in TRequest, out TResponse> where TRequest : QueryRequest<TResponse>
    {
        TResponse Handle(TRequest request);
    }
}