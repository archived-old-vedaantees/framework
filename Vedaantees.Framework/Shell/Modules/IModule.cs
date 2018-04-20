using Vedaantees.Framework.Providers.DependencyInjection;
using Vedaantees.Framework.Types.Results;

namespace Vedaantees.Framework.Shell.Modules
{
    /// <summary>
    ///     Implemented by every module to register all the classes to container
    /// </summary>
    public interface IModule
    {
        /// <summary>
        ///     Initializes the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        MethodResult Initialize(IContainerBuilder container);
    }
}