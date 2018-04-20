using System.Threading.Tasks;

namespace Vedaantees.Framework.Providers.Communications.ServiceBus
{
    public interface IEventHandler<in TEvent> where TEvent : class, IEvent
    {
        Task Handle(TEvent @event);
    }
}