using System.Collections.Generic;
using Vedaantees.Framework.Providers.Storages;

namespace Vedaantees.Framework.Providers.Communications.ServiceBus
{
    public class CommandSettings : IEntity<string>
    {
        public CommandSettings()
        {
            Settings = new List<CommandSetting>();
        }

        public List<CommandSetting> Settings { get; set; }
        public string Id { get; set; }
    }
}