using System;
using System.Threading.Tasks;
using Vedaantees.Framework.Providers.Communications.ServiceBus;
using Vedaantees.Framework.Providers.Logging;
using Rebus.Handlers;
using Rebus.DataBus;

namespace Vedaantees.Framework.Providers.ServiceBus.Consumers
{
    public class CommandHandlers<TMessage, TConsumer> : IHandleMessages<TMessage>
                                                        where TMessage  : Command
                                                        where TConsumer : ICommandHandler<TMessage>
    {
        private readonly TConsumer _consumer;
        private readonly ILogger _logger;

        public CommandHandlers(TConsumer consumer, ILogger logger)
        {
            _consumer = consumer;
            _logger = logger;
        }
        
        public async Task Handle(TMessage message)
        {
            _logger.Information("Executing handlers for command {0} & {1}", message.RequestId, _consumer);

            try
            {
                foreach(var attachment in message.Attachments)
                    attachment.FileStream = await DataBusAttachment.OpenRead(attachment.Id);
                
                await _consumer.Handle(message);
            }
            catch (Exception e)
            {
                _logger.Information("Error executing handler for command {0}", e);
                throw;
            }
        }
    }
}
