using System.Collections.Generic;
using Autofac;
using Vedaantees.Framework.Providers.Communications.ServiceBus;
using Vedaantees.Framework.Providers.Logging;
using Vedaantees.Framework.Types.Results;

namespace Vedaantees.Framework.Providers.ServiceBus
{
    public class TransformationService : ITransformationService
    {
        private readonly IComponentContext _container;
        private readonly ILogger _logger;

        public TransformationService(IComponentContext container, ILogger logger)
        {
            _container = container;
            _logger = logger;
        }

        public MethodResult PostProcess<TResponse>(TResponse response)
        {
            _logger.Information($@"response: {response.GetType().Name}.");
            var instance = typeof(IPostProcess<>).MakeGenericType(response.GetType());

            if (_container.IsRegistered(instance))
            {
                var transformer = _container.Resolve(instance);
                _logger.Information($@"Transformer: {transformer.GetType().Name}");

                transformer.GetType()
                           .GetMethod("Transform", new[] { response.GetType() })
                           .Invoke(transformer, new object[] { response });
            }

            return new MethodResult(MethodResultStates.Successful);
        }

        public MethodResult PreProcess<TRequest>(TRequest request)
        {
            _logger.Information($@"Request: {request.GetType().Name}.");
            var instance = typeof(IPreProcess<>).MakeGenericType(request.GetType());

            if (_container.IsRegistered(instance))
            {
                var transformer = _container.Resolve(instance);
                _logger.Information($@"Transformer: {transformer.GetType().Name}");

                transformer.GetType()
                           .GetMethod("Transform", new[] { request.GetType() })
                           .Invoke(transformer, new object[] { request });
            }

            return new MethodResult(MethodResultStates.Successful);
        }
    }
}