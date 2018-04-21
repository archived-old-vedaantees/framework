using Vedaantees.Framework.Shell.Modules;
using Vedaantees.Framework.Types.Results;

namespace Vedaantees.Framework.Providers
{
    public class RunAfterLoadingIsComplete : IRunAfterLoadingIsComplete
    {
        public MethodResult Run()
        {
            return new MethodResult(MethodResultStates.Successful);
        }

        public LoadingExecutionPriority GetPriority()
        {
             return LoadingExecutionPriority.First;
        }
    }
}
