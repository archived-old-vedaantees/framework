using Vedaantees.Framework.Types.Results;

namespace Vedaantees.Framework.Shell.Modules
{
    /// <summary>
    ///     Once initializating module is complete - use this interface to run post initialization code.
    /// </summary>
    public interface IRunAfterLoadingIsComplete
    {
        /// <summary>
        ///     Runs this instance.
        /// </summary>
        /// <returns></returns>
        MethodResult Run();

        /// <summary>
        ///     Gets the priority for execution.
        /// </summary>
        /// <returns></returns>
        LoadingExecutionPriority GetPriority();
    }
}