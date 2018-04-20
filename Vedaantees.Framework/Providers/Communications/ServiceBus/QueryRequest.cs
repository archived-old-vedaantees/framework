using System;

namespace Vedaantees.Framework.Providers.Communications.ServiceBus
{
    public abstract class QueryRequest<TResponse>
    {
        protected QueryRequest()
        {
            RequestId = Guid.NewGuid().ToString();
            RequestedOn = DateTime.Now;
        }

        public string RequestId { get; }
        public DateTime RequestedOn { get; }
        public string RequestedBy { get; set; }
        public TResponse Response { get; set; }
    }
}