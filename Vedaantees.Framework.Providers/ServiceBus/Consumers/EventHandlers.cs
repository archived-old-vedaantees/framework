using System.Threading.Tasks;
using Vedaantees.Framework.Providers.Communications.ServiceBus;
using Rebus.Handlers;

namespace Vedaantees.Framework.Providers.ServiceBus.Consumers
{
    public class EventHandlers<TMessage, TConsumer> : IHandleMessages<TMessage>
                                                      where TMessage : class, IEvent
                                                      where TConsumer : IEventHandler<TMessage>
    {
        private readonly TConsumer _consumer;

        public EventHandlers(TConsumer consumer)
        {
            _consumer = consumer;
        }

        public Task Handle(TMessage message)
        {
            return _consumer.Handle(message);
        }
    }
}