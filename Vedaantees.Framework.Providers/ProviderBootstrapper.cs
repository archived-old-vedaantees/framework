using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Vedaantees.Framework.Configurations;
using Vedaantees.Framework.Providers.Communications.Rest;
using Vedaantees.Framework.Providers.Communications.ServiceBus;
using Vedaantees.Framework.Providers.DependencyInjection;
using Vedaantees.Framework.Providers.Logging;
using Vedaantees.Framework.Providers.Mailing;
using Vedaantees.Framework.Providers.Rest;
using Vedaantees.Framework.Providers.Security;
using Vedaantees.Framework.Providers.ServiceBus;
using Vedaantees.Framework.Providers.ServiceBus.Consumers;
using Vedaantees.Framework.Providers.SingleSignOn;
using Vedaantees.Framework.Providers.Storages;
using Vedaantees.Framework.Providers.Storages.Data;
using Vedaantees.Framework.Providers.Storages.Graphs;
using Vedaantees.Framework.Providers.Storages.Keys;
using Vedaantees.Framework.Providers.Storages.Sessions;
using Vedaantees.Framework.Providers.Tasks;
using Vedaantees.Framework.Providers.Users;
using Vedaantees.Framework.Shell.Modules;
using Vedaantees.Framework.Shell.Rules;
using Vedaantees.Framework.Shell.UserContexts;
using Vedaantees.Framework.Types.Results;
using Vedaantees.Framework.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Neo4jClient;
using Rebus.Config;
using Rebus.Handlers;
using Rebus.Routing.TypeBased;
using Rebus.Transport;
using Serilog;
using ILogger = Vedaantees.Framework.Providers.Logging.ILogger;
using Rebus.DataBus;
using Rebus.DataBus.FileSystem;
using Rebus.Transport.InMem;
using Vedaantees.Framework.Providers.Storages.NullStores;

namespace Vedaantees.Framework.Providers
{
    /// <summary>
    /// Bootstraps the provider services.
    /// </summary>
    /// 
    public class ProviderBootstrapper
    {
        private readonly ContainerBuilder _autofacContainerBuilder;
        private readonly IContainerBuilder _containerBuilder;
        private readonly ProviderConfiguration _configuration;
        private readonly List<Assembly> _assemblies;
        public List<Assembly> GetAssemblies => _assemblies;

        public ProviderBootstrapper(IContainerBuilder containerBuilder, ProviderConfiguration configuration)
        {
            _containerBuilder = containerBuilder;
            _autofacContainerBuilder = ((AutofacContainerBuilder)containerBuilder).Builder;
            _configuration = configuration;
            _assemblies = new List<Assembly>();
            LoadAllReferencedAssemblies(configuration);
        }

        private void LoadAllReferencedAssemblies(ProviderConfiguration configuration)
        {
            var exclusions = new List<string> { "Vedaantees.Framework.dll", "Vedaantees.Framework.Providers.dll" };
            var modulesPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().GetName().CodeBase.Replace("file:///", string.Empty));
            var assemblies = new List<string>();
            assemblies.AddRange(Directory.GetFiles(modulesPath.Replace("file:\\", ""), "*.dll"));
            assemblies.AddRange(Directory.GetFiles(modulesPath.Replace("file:\\", ""), "*.exe"));

            if(!string.IsNullOrEmpty(configuration.ModulesFolder))
                assemblies.AddRange(Directory.GetFiles(configuration.ModulesFolder, "*.dll"));

            foreach (var assemblyName in assemblies)
            {
                var fileInfo = new FileInfo(assemblyName);

                if (exclusions.Contains(fileInfo.Name))
                    continue;

                if (DoesFileNameStartsWith(fileInfo.Name, configuration) && !fileInfo.Name.ToLower().Contains("vshost"))
                    _assemblies.Add(Assembly.LoadFrom(assemblyName));
            }
        }

        private bool DoesFileNameStartsWith(string fileName, ProviderConfiguration configuration)
        {
            foreach (var pattern in configuration.ScanAssembliesWithNamesStartingWith)
                if (fileName.StartsWith(pattern))
                    return true;

            return false;
        }

        public static IConfigurationRoot BuildConfiguration(IHostingEnvironment env)
        {
            var configurationBuilder = new ConfigurationBuilder()
                                           .AddJsonFile(GetGlobalSettingsPath(env), false, true)
                                           .SetBasePath(env.ContentRootPath)
                                           .AddJsonFile("appsettings.json", false, true)
                                           .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

            return configurationBuilder.Build();
        }

        private static string GetGlobalSettingsPath(IHostingEnvironment env) => new ConfigurationBuilder()
                                                                                    .SetBasePath(env.ContentRootPath)
                                                                                    .AddJsonFile("appsettings.json", false, true)
                                                                                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                                                                                    .Build()
                                                                                    .GetSection("Global-Settings")?
                                                                                    .Get<string>();
        
        public MethodResult Load(IHostingEnvironment env)
        {
            RegisterConfiguration();
            var logger = RegisterLogger(env.ContentRootPath);
            RegisterDocumentStore(logger);
            RegisterSqlStore(logger);
            RegisterGraphStore(logger);
            RegisterKeyGenerator();
            RegisterIdHasher();
            RegisterKeyGenerator();
            RegisterEmailServices();
            RegisterBus(logger);
            RegisterCommands();
            RegisterEvents();
            RegisterQueries();
            RegisterTransformers();
            RegisterRules();
            RegisterUserService();
            RegisterSecurity();
            RegisterRestServices();
            RegisterModules();
            RegisterRunAfterLoadingIsComplete();
            RegisterTasks();
            RegisterSingleSignOnRegistrar();

            return new MethodResult(MethodResultStates.Successful);
        }

        private void RegisterConfiguration()
        {
            _autofacContainerBuilder.RegisterInstance(_configuration);
        }

        private void RegisterSingleSignOnRegistrar()
        {
            _autofacContainerBuilder.RegisterType<SingleSignOnRegistrar>().As<SingleSignOnRegistrar>();
        }

        public void RegisterTasks()
        {
            _autofacContainerBuilder.RegisterType<TasksManager>().As<TasksManager>();

            var definitions = GetEntitiesInheritingFrom(typeof(ITask));
            foreach (var definition in definitions)
                _autofacContainerBuilder.RegisterType(definition).As<ITask>();
        }

        private void RegisterRestServices()
        {
            _autofacContainerBuilder.RegisterType<ApiClient>().As<IApiClient>();
        }

        private void RegisterRunAfterLoadingIsComplete()
        {
            var definitions = GetEntitiesInheritingFrom(typeof(IRunAfterLoadingIsComplete));

            foreach (var definition in definitions)
                _autofacContainerBuilder.RegisterType(definition).As<IRunAfterLoadingIsComplete>();
        }

        private void RegisterModules()
        {
            var modules = GetEntitiesInheritingFrom(typeof(IModule));

            foreach (var module in modules)
            {
                if (module.IsInterface) continue;

                var instance = Activator.CreateInstance(module) as IModule;
                instance?.Initialize(_containerBuilder);
            }
        }

        private void RegisterSecurity()
        {
            _autofacContainerBuilder.RegisterType<Sha256CryptoServiceProviderAdapter>()
                             .As<ICryptographicService>()
                             .InstancePerDependency();

            _autofacContainerBuilder.RegisterType<RecaptchaValidator>()
                             .As<IRecaptchaValidator>()
                             .InstancePerDependency();
        }

        private void RegisterUserService()
        {
            _autofacContainerBuilder.RegisterType<UserContextService>()
                             .As<IUserContextService>()
                             .InstancePerDependency();
        }

        private void RegisterRules()
        {
            _autofacContainerBuilder.RegisterType<RuleManager>()
                             .As<IRuleManager>()
                             .InstancePerLifetimeScope();

            foreach (var assembly in _assemblies)
                foreach (var type in assembly.GetTypes().ToList())
                {
                    var rule = type.GetInterface(typeof(IRule<>).Name);

                    if (rule != null)
                        _autofacContainerBuilder.RegisterType(type)
                                         .As(typeof(IRule<>).MakeGenericType(rule.GenericTypeArguments[0]))
                                         .InstancePerDependency();
                }
        }

        private void RegisterTransformers()
        {
            _autofacContainerBuilder.RegisterType<TransformationService>()
                .As<ITransformationService>()
                .InstancePerLifetimeScope();

            foreach (var assembly in _assemblies)
                foreach (var type in assembly.GetTypes().ToList())
                {
                    var preProcessor = type.GetInterface(typeof(IPreProcess<>).Name);
                    var postProcessor = type.GetInterface(typeof(IPostProcess<>).Name);

                    if (preProcessor != null)
                        _autofacContainerBuilder.RegisterType(type)
                                                .As(typeof(IPreProcess<>)
                                                .MakeGenericType(preProcessor.GenericTypeArguments[0]))
                                                .InstancePerDependency();

                    if (postProcessor != null)
                        _autofacContainerBuilder.RegisterType(type)
                                                .As(typeof(IPostProcess<>)
                                                .MakeGenericType(postProcessor.GenericTypeArguments[0]))
                                                .InstancePerDependency();
                }
        }

        private void RegisterBus(ILogger logger)
        {
            _autofacContainerBuilder.RegisterRebus((configurer, context) =>
            {
                var c = configurer
                    .Routing(r =>
                    {
                        var busSettingsManager = context.Resolve<IBusSettingsManager>();
                        var commands = GetEntitiesInheritingFrom(typeof(Command));
                        var registeredCommands = new List<string>();
                        var routerConfigurationBuilder = r.TypeBased();

                        if (_configuration.Bus.AcceptableCommmandsOrEventsFromNamespacesStartingWith != null &&
                            _configuration.Bus.AcceptableCommmandsOrEventsFromNamespacesStartingWith.Any() &&
                            !_configuration.Bus.IsSendOnly)
                        {
                            var settings = new CommandSettings();
                            var commandsInNamespace = GetEntitiesInheritingFrom(typeof(Command))
                                .Where(p => ContainsNamespace(p.FullName));

                            foreach (var command in commandsInNamespace)
                            {
                                settings.Settings.Add(new CommandSetting
                                {
                                    CommandName = command.FullName,
                                    Endpoint = $"{_configuration.Bus.Name}"
                                });
                            }

                            busSettingsManager.UpdateCommandSettings(settings);
                        }

                        var commandSettings = busSettingsManager.GetCommandSettings() ?? new CommandSettings();

                        foreach (var command in commands)
                        {
                            if (registeredCommands.Contains(command.FullName))
                                continue;

                            registeredCommands.Add(command.FullName);
                            var endpoint =
                                commandSettings.Settings.FirstOrDefault(p => p.CommandName == command.FullName);

                            if (endpoint != null)
                                routerConfigurationBuilder = routerConfigurationBuilder.Map(command, endpoint.Endpoint);
                        }
                    })
                    .Logging(l => l.Serilog(Log.Logger))
                    .Options(o =>
                    {
                        var isDocumentStoreRegistered = context.IsRegistered<Raven.Client.Documents.IDocumentStore>();
                        var isSqlStoreRegistered = context.IsRegistered<ISqlStore>() &&
                                                   !(context.Resolve<ISqlStore>() is NullSqlStore);
                        var isGraphStoreRegistered = context.IsRegistered<IGraphClientFactory>();

                        var unitOfWork = new UnitOfWorkFactory(GetEntitiesInheritingFrom(typeof(IEntity<long>)),
                            context.Resolve<ILogger>(),
                            isDocumentStoreRegistered ? context.Resolve<Raven.Client.Documents.IDocumentStore>() : null,
                            isSqlStoreRegistered ? _configuration.SqlStore : null,
                            isGraphStoreRegistered ? context.Resolve<IGraphClientFactory>() : null);

                        o.EnableUnitOfWork(unitOfWork.Create, unitOfWork.Commit, cleanupAction: unitOfWork.Cleanup);
                        o.EnableDataBus().StoreInFileSystem(_configuration.Bus.SharedFilePath);
                    })
                    .Options(o =>
                    {
                        o.SetNumberOfWorkers(2);
                        o.SetMaxParallelism(30);
                    });
                
                var verifier = new ProviderVerifier();

                if (!string.IsNullOrEmpty(_configuration.Bus.Endpoint) && verifier.IsQueueRunning(_configuration.Bus.Endpoint, logger))
                    c.Transport(t => t.UseRabbitMq(_configuration.Bus.Endpoint, _configuration.Bus.Name));
                else
                    c.Transport(t => t.UseInMemoryTransport(new InMemNetwork(), _configuration.Bus.Name));
                
                return c;
            });


            _autofacContainerBuilder.RegisterInstance(_configuration.Bus);
            _autofacContainerBuilder.RegisterType<Bus>()
                                     .As<ICommandService>()
                                     .As<IEventBus>()
                                     .As<IQueryService>()
                                     .InstancePerLifetimeScope();

            _autofacContainerBuilder.RegisterType<BusSettingsManager>().As<IBusSettingsManager>().InstancePerDependency();
        }

        private bool ContainsNamespace(string @namespace)
        {
            foreach (var name in _configuration.Bus.AcceptableCommmandsOrEventsFromNamespacesStartingWith)
                if (@namespace.StartsWith(name))
                    return true;

            return false;
        }

        private void RegisterEmailServices()
        {
            _autofacContainerBuilder.RegisterInstance(_configuration.Emails);
            _autofacContainerBuilder.RegisterType<EmailProvider>().As<IEmailProvider>().InstancePerLifetimeScope();
            _autofacContainerBuilder.RegisterType<TemplateBuilder>().As<ITemplateBuilder>().InstancePerLifetimeScope();
            _autofacContainerBuilder.RegisterType<SendEmailTask>().As<ITask>().AsSelf().InstancePerDependency();
        }

        private void RegisterKeyGenerator()
        {
            _autofacContainerBuilder.RegisterType<GenerateKey>()
                             .As<IGenerateKey>()
                             .InstancePerDependency();
        }

        private void RegisterIdHasher()
        {
            _autofacContainerBuilder.Register(ctx => new HashKeys(_configuration.EncryptionSalt))
                             .As<IHashKeys>()
                             .SingleInstance();
        }

        private void RegisterDocumentStore(ILogger logger)
        {
            var verifier = new ProviderVerifier();
            if (_configuration.DocumentStore==null || string.IsNullOrEmpty(_configuration.DocumentStore?.Url) || !verifier.IsNoSqlServiceRunning(_configuration.DocumentStore?.Url, 
                                                                                                                                        _configuration.DocumentStore?.Username, 
                                                                                                                                        _configuration.DocumentStore?.Password, 
                                                                                                                                        logger))
            {
                _autofacContainerBuilder.RegisterType<NullDocumentStore>()
                                        .As<IDocumentStore>()
                                        .SingleInstance();
                return;
            }

            var store = new Raven.Client.Documents.DocumentStore { Urls = new[] { _configuration.DocumentStore?.Url } };
            store.Initialize();

            _autofacContainerBuilder.RegisterInstance<Raven.Client.Documents.DocumentStore>(store)
                                    .As<Raven.Client.Documents.IDocumentStore>()
                                    .SingleInstance();

            _autofacContainerBuilder.Register(ctx =>
            {
                var transactionContext = AmbientTransactionContext.Current;
                var unitOfWork = (UnitOfWork)transactionContext?.Items["UnitOfWork"];

                if (unitOfWork != null)
                    return unitOfWork.DocumentSessionFactory;

                return new DocumentSessionFactory(ctx.Resolve<Raven.Client.Documents.IDocumentStore>(), ctx.Resolve<ILogger>(), false);
            })
                             .AsSelf()
                             .InstancePerLifetimeScope();

            _autofacContainerBuilder.RegisterType<DocumentStore>()
                             .As<IDocumentStore>()
                             .InstancePerDependency();
        }
                
        private void RegisterSqlStore(ILogger logger)
        {
            var verifier = new ProviderVerifier();
            if (_configuration.SqlStore == null || string.IsNullOrEmpty(_configuration.SqlStore?.ConnectionString) || !verifier.IsSqlServiceRunning(_configuration.SqlStore?.ConnectionString, logger))
            {
                _autofacContainerBuilder.RegisterType<NullSqlStore>()
                    .As<ISqlStore>()
                    .SingleInstance();
                return;
            }

            _autofacContainerBuilder.Register(ctx =>
                            {
                                var transactionContext = AmbientTransactionContext.Current;
                                var unitOfWork = (UnitOfWork)transactionContext?.Items["UnitOfWork"];

                                if (unitOfWork != null)
                                    return unitOfWork.SqlStore;

                                return new SqlStore(GetEntitiesInheritingFrom(typeof(IEntity<long>)), _configuration.SqlStore, false);
                            })
                             .As<ISqlStore>()
                             .InstancePerLifetimeScope();
        }
        
        private void RegisterGraphStore(ILogger logger)
        {
            var verifier = new ProviderVerifier();
            if (_configuration.GraphStore == null || 
                string.IsNullOrEmpty(_configuration.GraphStore?.Url) || 
                !verifier.IsGraphServiceRunning(_configuration.GraphStore?.Url,
                                                _configuration.GraphStore?.Username,
                                                _configuration.GraphStore?.Password,
                                                logger))
            {
                _autofacContainerBuilder.RegisterType<NullGraphStore>()
                                        .As<IGraphStore>()
                                        .SingleInstance();
                return;
            }

            _autofacContainerBuilder.Register(context => NeoServerConfiguration.GetConfiguration(new Uri($"{_configuration.GraphStore.Url}/db/data"),
                                                                                                            _configuration.GraphStore.Username,
                                                                                                            _configuration.GraphStore.Password))
                                    .SingleInstance();

            _autofacContainerBuilder.RegisterType<GraphClientFactory>()
                                    .As<IGraphClientFactory>()
                                    .SingleInstance();

            _autofacContainerBuilder.Register(ctx =>
            {
                var transactionContext = AmbientTransactionContext.Current;
                var unitOfWork = (UnitOfWork)transactionContext?.Items["UnitOfWork"];

                return unitOfWork != null ? unitOfWork.GraphStore :
                                            new GraphStore(ctx.Resolve<IGraphClientFactory>().Create(), false);
            })
                                    .As<IGraphStore>()
                                    .InstancePerLifetimeScope();
        }

        private ILogger RegisterLogger(string logPath)
        {
            var instance = new Logger(_configuration.Logging, _configuration.AppName, _configuration.Emails, logPath);
            _autofacContainerBuilder.RegisterInstance(instance).As<ILogger>();
            return instance;
        }

        private void RegisterQueries()
        {
            foreach (var assembly in _assemblies)
                foreach (var type in assembly.GetTypes().ToList())
                {
                    var queryHandlers = type.GetInterfaces().Where(p => p.IsGenericType && p.GetGenericTypeDefinition().Name == typeof(IQuery<,>).Name);

                    foreach (var queryHandler in queryHandlers)
                        _autofacContainerBuilder.RegisterType(type)
                                         .As(typeof(IQuery<,>).MakeGenericType(queryHandler.GenericTypeArguments[0], queryHandler.GenericTypeArguments[1]))
                                         .InstancePerLifetimeScope();
                }
        }

        private void RegisterCommands()
        {
            foreach (var assembly in _assemblies)
            {
                foreach (var type in assembly.GetTypes().ToList())
                {
                    var commandHandlerTypes = type.GetInterfaces().Where(p => p.IsSubClassOfGeneric(typeof(ICommandHandler<>)));

                    foreach (var commandHandlerType in commandHandlerTypes)
                    {
                        var massTransitConsumer = typeof(CommandHandlers<,>).MakeGenericType(commandHandlerType.GenericTypeArguments[0], type);

                        _autofacContainerBuilder.RegisterType(massTransitConsumer)
                                                .As(typeof(IHandleMessages<>)
                                                .MakeGenericType(commandHandlerType.GenericTypeArguments[0]))
                                                .InstancePerLifetimeScope();

                        _autofacContainerBuilder.RegisterType(type).InstancePerLifetimeScope();
                    }
                }
            }
        }

        private void RegisterEvents()
        {
            foreach (var assembly in _assemblies)
            {
                foreach (var type in assembly.GetTypes().ToList())
                {
                    var eventHandlerTypes = type.GetInterfaces().Where(p => p.IsSubClassOfGeneric(typeof(IEventHandler<>)));

                    foreach (var eventHandlerType in eventHandlerTypes)
                    {
                        var massTransitConsumer = typeof(EventHandlers<,>)
                                                        .MakeGenericType(eventHandlerType.GenericTypeArguments[0], typeof(IEventHandler<>).MakeGenericType(eventHandlerType.GenericTypeArguments[0]));

                        _autofacContainerBuilder.RegisterType(massTransitConsumer)
                                         .As(typeof(IHandleMessages<>)
                                         .MakeGenericType(eventHandlerType.GenericTypeArguments[0]))
                                         .InstancePerLifetimeScope();

                        _autofacContainerBuilder.RegisterType(type)
                                         .As(typeof(IEventHandler<>)
                                         .MakeGenericType(eventHandlerType.GenericTypeArguments[0]))
                                         .InstancePerLifetimeScope();
                    }
                }
            }
        }

        public void InitializeFramework(Autofac.IContainer container)
        {
            RunAfterLoadingIsComplete(container.Resolve<IEnumerable<IRunAfterLoadingIsComplete>>());
        }

        private void RunAfterLoadingIsComplete(IEnumerable<IRunAfterLoadingIsComplete> modules)
        {
            foreach (var module in modules.OrderBy(p => p.GetPriority()))
                module.Run();
        }

        private List<Type> GetEntitiesInheritingFrom(Type memberInfo)
        {
            return _assemblies.SelectMany(assembly => assembly.GetTypes().Where(p => p.IsSubClassOfGeneric(memberInfo) && !p.IsInterface)).ToList();
        }
    }
}