using Vedaantees.Framework.Types.Results;

namespace Vedaantees.Framework.Providers.Communications.ServiceBus
{
    public interface ITransformationService
    {
        MethodResult PreProcess<TRequest>(TRequest request);
        MethodResult PostProcess<TResponse>(TResponse response);
    }
}