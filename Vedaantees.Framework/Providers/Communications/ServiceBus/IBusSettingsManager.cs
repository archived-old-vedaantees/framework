namespace Vedaantees.Framework.Providers.Communications.ServiceBus
{
    public interface IBusSettingsManager
    {
        CommandSettings GetCommandSettings();
        void UpdateCommandSettings(CommandSettings settings);
    }
}