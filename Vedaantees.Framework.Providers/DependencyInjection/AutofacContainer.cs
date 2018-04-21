using Autofac;

namespace Vedaantees.Framework.Providers.DependencyInjection
{
    public class AutofacContainer : IContainer
    {
        private readonly IComponentContext _container;

        public AutofacContainer(IComponentContext container)
        {
            _container = container;
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public IComponentContext GetContainer(){ return _container; }
    }
}