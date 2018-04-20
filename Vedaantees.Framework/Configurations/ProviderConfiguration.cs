using System.Collections.Generic;
using Vedaantees.Framework.Providers.Communications.ServiceBus;
using Vedaantees.Framework.Providers.Logging;
using Vedaantees.Framework.Providers.Mailing;
using Vedaantees.Framework.Providers.Storages.Data;

namespace Vedaantees.Framework.Configurations
{
    public class ProviderConfiguration
    {
        public ProviderConfiguration()
        {
            Logging = new LoggerConfiguration();
            DocumentStore = new DocumentStoreSetting();
            SqlStore = new SqlStoreSetting();
            GraphStore = new GraphStoreSetting();
            Emails = new EmailSettings();
            Bus = new BusSetting();
        }

        public LoggerConfiguration Logging { get; set; }

        public EmailSettings Emails { get; set; }

        public DocumentStoreSetting DocumentStore { get; set; }

        public SqlStoreSetting SqlStore { get; set; }

        public GraphStoreSetting GraphStore { get; set; }

        public BusSetting Bus { get; set; }

        public List<string> ScanAssembliesWithNamesStartingWith { get; set; }

        public string AppName { get; set; }

        public string EncryptionSalt { get; set; }

        public string ModulesFolder { get; set; }

        public string SingleSignOnServer { get; set; }
    }
}