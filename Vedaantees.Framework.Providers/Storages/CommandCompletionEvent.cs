using Vedaantees.Framework.Providers.Communications.ServiceBus;

namespace Vedaantees.Framework.Providers.Storages
{
    public class CommandCompletionEvent : IEvent
    {
        public string RequestId { get; set; }
    }
}