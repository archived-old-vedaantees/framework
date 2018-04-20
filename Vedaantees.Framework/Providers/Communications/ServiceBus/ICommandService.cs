using System.Collections.Generic;
using System.Threading.Tasks;
using Vedaantees.Framework.Types.Results;

namespace Vedaantees.Framework.Providers.Communications.ServiceBus
{
    public interface ICommandService
    {
        Task<MethodResult<TCommand>> ExecuteCommand<TCommand>(TCommand command) where TCommand : Command;

        Task<MethodResult<TCommand>> ExecuteCommandWithAttachments<TCommand>(TCommand command, IList<Attachment> files)
            where TCommand : Command;
    }
}