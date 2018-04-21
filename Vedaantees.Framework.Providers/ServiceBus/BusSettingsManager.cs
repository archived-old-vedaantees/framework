using System.Linq;
using Vedaantees.Framework.Providers.Communications.ServiceBus;
using Vedaantees.Framework.Providers.Storages.Data;

namespace Vedaantees.Framework.Providers.ServiceBus
{
    public class BusSettingsManager : IBusSettingsManager
    {
        private readonly IDocumentStore _documentStore;

        public BusSettingsManager(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
            _documentStore.SetSession("Master");
        }

        public CommandSettings GetCommandSettings()
        {
            return _documentStore.First<CommandSettings>();
        }

        public void UpdateCommandSettings(CommandSettings settings)
        {
            var commandSettings = GetCommandSettings() ?? new CommandSettings();

            foreach (var commandSetting in settings.Settings)
            {
                var setting = commandSettings.Settings.FirstOrDefault(p => p.CommandName == commandSetting.CommandName);

                if (setting != null)
                    commandSetting.Endpoint = setting.Endpoint;
                else
                    commandSettings.Settings.Add(commandSetting);
            }

            _documentStore.Store(commandSettings);
        }
    }
}