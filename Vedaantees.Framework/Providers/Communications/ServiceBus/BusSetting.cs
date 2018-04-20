using System.Collections.Generic;

namespace Vedaantees.Framework.Providers.Communications.ServiceBus
{
    public class BusSetting
    {
        public string Endpoint { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public List<string> AcceptableCommmandsOrEventsFromNamespacesStartingWith { get; set; }
        public bool IsSendOnly { get; set; }
        public string SharedFilePath { get; set; }
    }
}