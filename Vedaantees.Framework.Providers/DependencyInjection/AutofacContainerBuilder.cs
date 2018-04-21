using System;
using System.Linq;
using System.Reflection;
using Autofac;

namespace Vedaantees.Framework.Providers.DependencyInjection
{
    public class AutofacContainerBuilder : IContainerBuilder
    {
        public ContainerBuilder Builder { get; private set; }

        public AutofacContainerBuilder()
        {
            Builder = new ContainerBuilder();
        }

        public void RegisterPerRequest<TInterface, TImplementation>() where TImplementation : class, TInterface
        {
            Builder.RegisterType<TImplementation>().As<TInterface>().InstancePerLifetimeScope();
        }

        public void RegisterSingleton<TInterface, TImplementation>() where TImplementation : class, TInterface
        {
            Builder.RegisterType<TImplementation>().As<TInterface>().SingleInstance();
        }

        public void RegisterInstance<TInterface>(TInterface instance) where TInterface : class
        {
            Builder.RegisterInstance(instance).As<TInterface>();
        }

        public void RegisterTransient<TInterface, TImplementation>() where TImplementation : class, TInterface
        {
            Builder.RegisterType<TImplementation>().As<TInterface>().InstancePerDependency();
        }

        public void RegisterPerRequest<TInterface1, TInterface2, TImplementation>() where TImplementation : class, TInterface1, TInterface2
        {
            Builder.RegisterType<TImplementation>()
                    .As<TInterface1>()
                    .As<TInterface2>()
                    .InstancePerLifetimeScope();
        }

        public void RegisterPerRequest<TInterface1, TInterface2, TInterface3, TImplementation>() where TImplementation : class, TInterface1, TInterface2, TInterface3
        {
            Builder.RegisterType<TImplementation>()
                    .As<TInterface1>()
                    .As<TInterface2>()
                    .As<TInterface3>()
                    .InstancePerLifetimeScope();
        }

        public void RegisterSingleton<TInterface1, TInterface2, TImplementation>() where TImplementation : class, TInterface1, TInterface2
        {
            Builder.RegisterType<TImplementation>()
                    .As<TInterface1>()
                    .As<TInterface2>()
                    .SingleInstance();
        }

        public void RegisterSingleton<TInterface1, TInterface2, TInterface3, TImplementation>() where TImplementation : class, TInterface1, TInterface2, TInterface3
        {
            Builder.RegisterType<TImplementation>()
                    .As<TInterface1>()
                    .As<TInterface2>()
                    .As<TInterface3>()
                    .SingleInstance();
        }

        public void RegisterInstance<TInterface, TImplementation>(TImplementation instance) where TImplementation : class, TInterface
        {
            Builder.RegisterInstance(instance)
                    .As<TInterface>();
        }

        public void RegisterInstance<TInterface1, TInterface2, TImplementation>(TImplementation instance) where TImplementation : class, TInterface1, TInterface2
        {
            Builder.RegisterInstance(instance)
                    .As<TInterface1>()
                    .As<TInterface2>();
        }

        public void RegisterInstance<TInterface1, TInterface2, TInterface3, TImplementation>(TImplementation instance) where TImplementation : class, TInterface1, TInterface2, TInterface3
        {
            Builder.RegisterInstance(instance)
                    .As<TInterface1>()
                    .As<TInterface2>()
                    .As<TInterface3>();
        }

        public void RegisterTransient<TInterface1, TInterface2, TImplementation>() where TImplementation : class, TInterface1, TInterface2
        {
            Builder.RegisterType<TImplementation>()
                    .As<TInterface1>()
                    .As<TInterface2>()
                    .InstancePerDependency();
        }

        public void RegisterTransient<TInterface1, TInterface2, TInterface3, TImplementation>() where TImplementation : class, TInterface1, TInterface2, TInterface3
        {
            Builder.RegisterType<TImplementation>()
                    .As<TInterface1>()
                    .As<TInterface2>()
                    .As<TInterface3>()
                    .InstancePerDependency();
        }

        public void RegisterPerRequest<TInterface, TImplementation>(Func<IContainer, TImplementation> func) where TImplementation : class, TInterface
        {
            Builder.Register(context => func(new AutofacContainer(context)))
                    .As<TInterface>()
                    .InstancePerLifetimeScope();
        }

        public void RegisterPerRequest<TInterface1, TInterface2, TImplementation>(Func<IContainer, TImplementation> func) where TImplementation : class, TInterface1, TInterface2
        {
            Builder.Register(context => func(new AutofacContainer(context)))
                    .As<TInterface1>()
                    .As<TInterface2>()
                    .InstancePerLifetimeScope();
        }

        public void RegisterPerRequest<TInterface1, TInterface2, TInterface3, TImplementation>(Func<IContainer, TImplementation> func) where TImplementation : class, TInterface1, TInterface2, TInterface3
        {
            Builder.Register(context => func(new AutofacContainer(context)))
                    .As<TInterface1>()
                    .As<TInterface2>()
                    .As<TInterface3>()
                    .InstancePerLifetimeScope();
        }

        public void RegisterSingleton<TInterface, TImplementation>(Func<IContainer, TImplementation> func) where TImplementation : class, TInterface
        {
            Builder.Register(context => func(new AutofacContainer(context)))
                    .As<TInterface>()
                    .SingleInstance();
        }

        public void RegisterSingleton<TInterface1, TInterface2, TImplementation>(Func<IContainer, TImplementation> func) where TImplementation : class, TInterface1, TInterface2
        {
            Builder.Register(context => func(new AutofacContainer(context)))
                    .As<TInterface1>()
                    .As<TInterface2>()
                    .SingleInstance();
        }

        public void RegisterSingleton<TInterface1, TInterface2, TInterface3, TImplementation>(Func<IContainer, TImplementation> func) where TImplementation : class, TInterface1, TInterface2, TInterface3
        {
            Builder.Register(context => func(new AutofacContainer(context)))
                    .As<TInterface1>()
                    .As<TInterface2>()
                    .As<TInterface3>()
                    .SingleInstance();
        }

        public void RegisterPerRequest<TInterface>(Type type)
        {
            Builder.RegisterType(type).As<TInterface>().InstancePerLifetimeScope();
        }

        public void RegisterPerRequest<TInterface1, TInterface2>(Type type)
        {
            Builder.RegisterType(type)
                    .As<TInterface1>()
                    .As<TInterface2>()
                    .InstancePerLifetimeScope();
        }

        public void RegisterPerRequest<TInterface1, TInterface2, TInterface3>(Type type)
        {
            Builder.RegisterType(type)
                   .As<TInterface1>()
                   .As<TInterface2>()
                   .As<TInterface3>()
                   .InstancePerLifetimeScope();
        }

        public void RegisterPerRequest(Type @interface, Type @class)
        {
            Builder.RegisterType(@class).As(@interface).InstancePerLifetimeScope();
        }

        public void RegisterTypesByThierInterfaces(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes().Where(p => !p.IsInterface))
                foreach (var @interface in type.GetInterfaces())
                    Builder.RegisterType(type)
                            .As(@interface)
                            .InstancePerLifetimeScope();
        }

        public Autofac.IContainer Build()
        {
            return Builder.Build();
        }
    }
}