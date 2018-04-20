using System.Threading.Tasks;
using Vedaantees.Framework.Types.Results;

namespace Vedaantees.Framework.Providers.Communications.ServiceBus
{
    public interface IEventBus
    {
        Task<MethodResult<TEvent>> Publish<TEvent>(TEvent @event) where TEvent : class;
        IEventBus Subscribe<TEvent>() where TEvent : class;
    }
}