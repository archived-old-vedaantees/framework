using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Vedaantees.Framework.Providers.Communications.ServiceBus;
using Vedaantees.Framework.Providers.Logging;
using Vedaantees.Framework.Providers.Storages.Keys;
using Vedaantees.Framework.Shell.Rules;
using Vedaantees.Framework.Types.Results;
using Rebus.Bus;

namespace Vedaantees.Framework.Providers.ServiceBus
{
    public class Bus : ICommandService, IEventBus, IQueryService

    {
        private readonly IBus _bus;
        private readonly ILogger _logger;
        private readonly IGenerateKey _generateKey;
        private readonly IComponentContext _componentContext;
        private readonly ITransformationService _transformationService;
        private readonly IRuleManager _ruleManager;
        
        public Bus(IBus bus, ILogger logger, IGenerateKey generateKey, IRuleManager ruleManager, IComponentContext componentContext, ITransformationService transformationService)
        {
            _bus = bus;
            _logger = logger;
            _generateKey = generateKey;
            _ruleManager = ruleManager;
            _componentContext = componentContext;
            _transformationService = transformationService;
        }

        public async Task<MethodResult<TEvent>> Publish<TEvent>(TEvent @event) where TEvent : class
        {
            var result = _bus.Publish(@event);
            await result;
            
            if(result.IsCanceled)
                return new MethodResult<TEvent>(MethodResultStates.UnSuccessful, "Event was cancelled");

            if(result.IsFaulted)
                return new MethodResult<TEvent>(result.Exception);

            return new MethodResult<TEvent>(MethodResultStates.Successful);
        }

        public async Task<MethodResult<TCommand>> ExecuteCommandWithAttachments<TCommand>(TCommand command, IList<Attachment> files) where TCommand : Command
        {
            foreach (var file in files)
            {
                var attachment = await _bus.Advanced.DataBus.CreateAttachment(file.FileStream);
                file.FileStream = null;
                file.Id = attachment.Id;

                command.Attachments.Add(file);
            }

           return await ExecuteCommand(command);
        }

        public async Task<MethodResult<TCommand>> ExecuteCommand<TCommand>(TCommand command) where TCommand : Command
        {
            MethodResult<TCommand> methodResult;
            _logger.Information("Executing command: {0}, Id:{1}.", command, command.RequestId);

            try
            {
                _transformationService.PreProcess(command);
                var businessRules = _ruleManager.ExecuteBusinessRules(command);

                if (!businessRules.IsSuccessful)
                {
                    methodResult = new MethodResult<TCommand>(MethodResultStates.UnSuccessful, $"{businessRules}");
                    _logger.Information("Execution failed for validations {0}. Reasons: {1}", command.RequestId, methodResult);
                }
                else
                {
                    var props = command.GetType()
                                       .GetProperties()
                                       .Where(prop => Attribute.IsDefined(prop, typeof(GenerateKeyAttribute)));

                    foreach (var prop in props)
                    {
                        var generateIdAttr = prop.GetCustomAttributes(true)
                            .OfType<GenerateKeyAttribute>()
                            .FirstOrDefault();

                        if (generateIdAttr != null)
                        {
                            if(prop.PropertyType==typeof(string))
                                prop.SetValue(command, $"{_generateKey.GetNextStringKey( generateIdAttr.CollectionName)}");

                            else if (prop.PropertyType == typeof(long))
                                prop.SetValue(command, _generateKey.GetNextNumericalKey(generateIdAttr.CollectionName));
                        }
                    }
                    _logger.Information("Sending command for execution {0}.", command.RequestId);
                    await _bus.Send(command);

                    methodResult = new MethodResult<TCommand>(command);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occured @ CommandService: ");
                methodResult = new MethodResult<TCommand>(MethodResultStates.UnSuccessful, "Error occured executing the command.");
            }

            return methodResult;
        }

        public MethodResult<TResponse> ExecuteQuery<TRequest, TResponse>(TRequest request) where TRequest : QueryRequest<TResponse> 
        {
            try
            {
                _logger.Information("Executing command: {0}, Id:{1}.", request, request.RequestId);

                var handlerType = typeof(IQuery<,>).MakeGenericType(request.GetType(), typeof(TResponse));
                var handler = _componentContext.Resolve(handlerType) as IQuery<TRequest, TResponse>;

                if (handler == null)
                    throw new Exception("Query executor for request not found");

                var response = handler.Handle(request);
                _transformationService.PostProcess(response);
                return new MethodResult<TResponse>(response);
            }
            catch (Exception exception)
            {
                _logger.Error($"Error occured processing query: {request.GetType().FullName}", exception, string.Empty, request);
                return new MethodResult<TResponse>(exception);
            }
        }

        public IEventBus Subscribe<TEvent>() where TEvent : class
        {
            _bus.Subscribe<TEvent>();
            return this;
        }
    }
}