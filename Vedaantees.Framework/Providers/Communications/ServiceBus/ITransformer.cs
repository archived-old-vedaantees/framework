using Vedaantees.Framework.Types.Results;

namespace Vedaantees.Framework.Providers.Communications.ServiceBus
{
    public interface IPreProcess<in TRequest>
    {
        MethodResult Transform(TRequest request);
    }

    public interface IPostProcess<TResponse>
    {
        MethodResult Transform(TResponse response);
    }
}