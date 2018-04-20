using System;
using System.Reflection;

namespace Vedaantees.Framework.Providers.DependencyInjection
{
    /// <summary>
    ///     This will help register elements to container.
    /// </summary>
    public interface IContainerBuilder
    {
        void RegisterPerRequest(Type @interface, Type @class);

        void RegisterPerRequest<TInterface>(Type type);

        void RegisterPerRequest<TInterface1, TInterface2>(Type type);

        void RegisterPerRequest<TInterface1, TInterface2, TInterface3>(Type type);

        void RegisterPerRequest<TInterface, TImplementation>() where TImplementation : class, TInterface;

        void RegisterPerRequest<TInterface1, TInterface2, TImplementation>()
            where TImplementation : class, TInterface1, TInterface2;

        void RegisterPerRequest<TInterface1, TInterface2, TInterface3, TImplementation>()
            where TImplementation : class, TInterface1, TInterface2, TInterface3;

        void RegisterSingleton<TInterface, TImplementation>() where TImplementation : class, TInterface;

        void RegisterSingleton<TInterface1, TInterface2, TImplementation>()
            where TImplementation : class, TInterface1, TInterface2;

        void RegisterSingleton<TInterface1, TInterface2, TInterface3, TImplementation>()
            where TImplementation : class, TInterface1, TInterface2, TInterface3;

        void RegisterInstance<TInterface, TImplementation>(TImplementation instance)
            where TImplementation : class, TInterface;

        void RegisterInstance<TInterface1, TInterface2, TImplementation>(TImplementation instance)
            where TImplementation : class, TInterface1, TInterface2;

        void RegisterInstance<TInterface1, TInterface2, TInterface3, TImplementation>(TImplementation instance)
            where TImplementation : class, TInterface1, TInterface2, TInterface3;

        void RegisterTransient<TInterface, TImplementation>() where TImplementation : class, TInterface;

        void RegisterTransient<TInterface1, TInterface2, TImplementation>()
            where TImplementation : class, TInterface1, TInterface2;

        void RegisterTransient<TInterface1, TInterface2, TInterface3, TImplementation>()
            where TImplementation : class, TInterface1, TInterface2, TInterface3;

        void RegisterPerRequest<TInterface, TImplementation>(Func<IContainer, TImplementation> func)
            where TImplementation : class, TInterface;

        void RegisterPerRequest<TInterface1, TInterface2, TImplementation>(Func<IContainer, TImplementation> func)
            where TImplementation : class, TInterface1, TInterface2;

        void RegisterPerRequest<TInterface1, TInterface2, TInterface3, TImplementation>(
            Func<IContainer, TImplementation> func)
            where TImplementation : class, TInterface1, TInterface2, TInterface3;

        void RegisterSingleton<TInterface, TImplementation>(Func<IContainer, TImplementation> func)
            where TImplementation : class, TInterface;

        void RegisterSingleton<TInterface1, TInterface2, TImplementation>(Func<IContainer, TImplementation> func)
            where TImplementation : class, TInterface1, TInterface2;

        void RegisterSingleton<TInterface1, TInterface2, TInterface3, TImplementation>(
            Func<IContainer, TImplementation> func)
            where TImplementation : class, TInterface1, TInterface2, TInterface3;

        void RegisterTypesByThierInterfaces(Assembly assembly);
    }
}