using Autofac;
using Vedaantees.Framework.Providers;
using Vedaantees.Framework.Providers.Communications.ServiceBus;
using Vedaantees.Framework.Providers.DependencyInjection;
using Vedaantees.Framework.Providers.Logging;
using Vedaantees.Framework.Providers.Mailing;
using Vedaantees.Framework.Providers.Storages.Data;
using Vedaantees.Framework.Providers.Storages.Graphs;
using Vedaantees.Framework.Providers.Storages.Keys;
using Vedaantees.Framework.Types.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rebus.Bus;

namespace Vedaantees.Framework.Tests
{
    [TestClass]
    public class BootstrapperTests
    {
        [TestMethod]
        public void InitializeCompletely()
        {
            var containerBuilder = new AutofacContainerBuilder();
            var bootstrapper = new ProviderBootstrapper(containerBuilder,MockBuilder.BuildConfiguration());
            var methodResult = bootstrapper.Load(new MockHosting());
            
            var container = containerBuilder.Build();
            bootstrapper.InitializeFramework(container);

            Assert.AreEqual(methodResult.MethodResultState, MethodResultStates.Successful);
            Assert.AreNotEqual(container.Resolve<ILogger>(), null);
            Assert.AreNotEqual(container.Resolve<IDocumentStore>(), null);
            Assert.AreNotEqual(container.Resolve<IGraphStore>(), null);
            Assert.AreNotEqual(container.Resolve<ISqlStore>(), null);
            Assert.AreNotEqual(container.Resolve<IGenerateKey>(), null);
            Assert.AreNotEqual(container.Resolve<IHashKeys>(), null);
            Assert.AreNotEqual(container.Resolve<IEmailProvider>(), null);
            Assert.AreNotEqual(container.Resolve<ITemplateBuilder>(), null);
            Assert.AreNotEqual(container.Resolve<IBus>(), null);
            Assert.AreNotEqual(container.Resolve<ICommandService>(), null);
            Assert.AreNotEqual(container.Resolve<IEventBus>(), null);
            Assert.AreNotEqual(container.Resolve<IQueryService>(), null);
        }

        [TestMethod]
        public void TestSendEmailTask()
        {
            var containerBuilder = new AutofacContainerBuilder();
            var bootstrapper = new ProviderBootstrapper(containerBuilder, MockBuilder.BuildConfiguration());
            var methodResult = bootstrapper.Load(new MockHosting());

            var container = containerBuilder.Build();
            bootstrapper.InitializeFramework(container);
            var sendEmailTask = container.Resolve<SendEmailTask>();
            var result = sendEmailTask.Execute().Result;
            Assert.AreEqual(result.MethodResultState,MethodResultStates.Successful);
            
        }
    }
}