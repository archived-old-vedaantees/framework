namespace Vedaantees.Framework.Providers.Logging
{
    public class LoggerConfiguration
    {
        public LoggerConfiguration()
        {
            EnableSerilogDebugger = false;
        }

        public bool EnableFile { get; set; }
        public bool EnableEmail { get; set; }
        public bool EnableWindowsEventViewer { get; set; }
        public bool EnableSerilogDebugger { get; set; }
        public string LoggingFromEmail { get; set; }
        public string LoggingToEmail { get; set; }
    }
}