using System.Collections.Generic;
using System.IO;
using Vedaantees.Framework.Configurations;
using Vedaantees.Framework.Providers.Communications.ServiceBus;
using Vedaantees.Framework.Providers.DependencyInjection;
using Vedaantees.Framework.Providers.Logging;
using Vedaantees.Framework.Providers.Mailing;
using Vedaantees.Framework.Providers.Storages.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Vedaantees.Framework.Tests
{
    public static class MockBuilder
    {
        public static ProviderConfiguration BuildConfiguration()
        {
            var providerConfiguration = new ProviderConfiguration
            {
                AppName = "Testing-Framework-Providers",
                ScanAssembliesWithNamesStartingWith = new List<string>{ "Vedaantees", "WorkNotes" },
                EncryptionSalt = "QSF#@$4F3@*&J55",
                Logging = new LoggerConfiguration
                {
                    EnableEmail = true,
                    EnableFile = true,
                    EnableSerilogDebugger = false,
                    LoggingFromEmail = "",
                    LoggingToEmail = ""
                },
                DocumentStore = new DocumentStoreSetting
                {
                    Url = "http://localhost:8080",
                    Username = string.Empty,
                    Password = string.Empty
                },
                SqlStore = new SqlStoreSetting
                {
                    ConnectionString = "Server=127.0.0.1;Port=5432;Database=postgres;User Id=postgres;Password=A8$bhaysn;",
                    DisableQueryTracking = false
                }
                ,
                GraphStore = new GraphStoreSetting
                {
                    Url = "http://localhost:7474",
                    Username = "neo4j",
                    Password = "A8$bhaysn"
                },
                Emails = new EmailSettings
                {
                    Server = "smtp.gmail.com",
                    Port = "587",
                    Username = "vedaantees@gmail.com",
                    Password = "2ed@@ntees"
                },
                Bus = new BusSetting
                {
                    Endpoint = "amqp://guest:guest@localhost",
                    Username = "guest",
                    Password = "guest",
                    Name     = "Test-ServiceBus",
                    IsSendOnly = false,
                    AcceptableCommmandsOrEventsFromNamespacesStartingWith = new List<string> { "Vedaantees.Framework.Tests" },
                    SharedFilePath = @"d:\_path"
                }
            };

            File.WriteAllText("configuration.json",JsonConvert.SerializeObject(providerConfiguration));
            return providerConfiguration;
        }

        public static Autofac.IContainer BuildContainer()
        {
            var containerBuilder = new AutofacContainerBuilder();
            //var bootstrapper = new ProviderBootstrapper(containerBuilder); //MockBuilder.BuildConfiguration()
            //bootstrapper.Load();
            return containerBuilder.Build();
        }
    }

    public class MockHosting : IHostingEnvironment
    {
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public string WebRootPath { get; set; }
        public IFileProvider WebRootFileProvider { get; set; }
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }
    }
}