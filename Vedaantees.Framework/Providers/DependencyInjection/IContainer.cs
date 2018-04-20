namespace Vedaantees.Framework.Providers.DependencyInjection
{
    public interface IContainer
    {
        T Resolve<T>();
    }
}