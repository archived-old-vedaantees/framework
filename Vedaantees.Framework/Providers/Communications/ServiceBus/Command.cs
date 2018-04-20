using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Vedaantees.Framework.Providers.Communications.ServiceBus
{
    public abstract class Command
    {
        protected Command()
        {
            RequestId = Guid.NewGuid().ToString();
            RequestedOn = DateTime.Now;
            Attachments = new List<Attachment>();
            Properties = new Dictionary<string, JToken>();
        }

        [JsonExtensionData] public IDictionary<string, JToken> Properties { get; set; }

        public string RequestId { get; }
        public DateTime RequestedOn { get; }
        public string RequestedBy { get; set; }
        public IList<Attachment> Attachments { get; }
    }

    public class Attachment
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Stream FileStream { get; set; }
    }
}