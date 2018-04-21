using Autofac;
using Vedaantees.Framework.Providers.DependencyInjection;
using Vedaantees.Framework.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Vedaantees.Framework.Tests
{
    [TestClass]
    public class DependencyInjectionTests
    {
        [TestMethod]
        public void ResolvePerRequest()
        {
            var containerBuilder = new AutofacContainerBuilder();
            containerBuilder.RegisterPerRequest<ITestInterfaceForContainer, TestInterfaceForContainer>();
            var container = containerBuilder.Build();
            var instance = container.Resolve<ITestInterfaceForContainer>();
            Assert.AreEqual(instance.GetType(),typeof(TestInterfaceForContainer));
        }

        [TestMethod]
        public void RegisterSingleton()
        {
            var containerBuilder = new AutofacContainerBuilder();
            containerBuilder.RegisterSingleton<ITestInterfaceForContainer, TestInterfaceForContainer>();
            var container = containerBuilder.Build();
            var instance = container.Resolve<ITestInterfaceForContainer>();
            Assert.AreEqual(instance.GetType(), typeof(TestInterfaceForContainer));
        }
        
        [TestMethod]
        public void ResolveInstance()
        {
            var containerBuilder = new AutofacContainerBuilder();
            containerBuilder.RegisterInstance<ITestInterfaceForContainer>(new TestInterfaceForContainer());
            var container = containerBuilder.Build();
            var instance = container.Resolve<ITestInterfaceForContainer>();
            Assert.AreEqual(instance.GetType(), typeof(TestInterfaceForContainer));
        }

        [TestMethod]
        public void ResolveTransient()
        {
            var containerBuilder = new AutofacContainerBuilder();
            containerBuilder.RegisterTransient<ITestGenericInterface<ITestInterfaceForContainer>,TestGeneric>();
            var container = containerBuilder.Build();
            var instance = container.Resolve<ITestGenericInterface<ITestInterfaceForContainer>>();
            Assert.AreEqual(instance.GetType(), typeof(TestGeneric));
        }

        [TestMethod]
        public void ResolveTypesByThierInterfaces()
        {
            var containerBuilder = new AutofacContainerBuilder();
            containerBuilder.RegisterTypesByThierInterfaces(GetType().Assembly);
            var container = containerBuilder.Build();
            var instance = container.Resolve<ITestGenericInterface<ITestInterfaceForContainer>>();
            Assert.AreEqual(instance.GetType(), typeof(TestGeneric));
        }

        [TestMethod]
        public void ResolveByFunc()
        {
            var containerBuilder = new AutofacContainerBuilder();
            containerBuilder.RegisterPerRequest<ITestInterfaceForContainer, TestInterfaceForContainer>(context => new TestInterfaceForContainer());
            var container = containerBuilder.Build();
            var instance = container.Resolve<ITestInterfaceForContainer>();
            Assert.AreEqual(instance.GetType(), typeof(TestInterfaceForContainer));
        }

        [TestMethod]
        public void ResolveByFuncInstance()
        {
            var containerBuilder = new AutofacContainerBuilder();
            containerBuilder.RegisterSingleton<ITestInterfaceForContainer, TestInterfaceForContainer>(context => new TestInterfaceForContainer());
            var container = containerBuilder.Build();
            var instance = container.Resolve<ITestInterfaceForContainer>();
            Assert.AreEqual(instance.GetType(), typeof(TestInterfaceForContainer));
        }

        [TestMethod]
        public void ResolveByMultipleFuncInstance()
        {
            var containerBuilder = new AutofacContainerBuilder();
            containerBuilder.RegisterSingleton<ITestInterfaceForContainer, IOtherTestInterfaceForContainer, TestInterfaceForContainer>(context => new TestInterfaceForContainer());
            var container = containerBuilder.Build();
            var instance = container.Resolve<IOtherTestInterfaceForContainer>();
            Assert.AreEqual(instance.GetType(), typeof(TestInterfaceForContainer));
        }
    }
    
    #region "test-helper-classes"

    #endregion
}