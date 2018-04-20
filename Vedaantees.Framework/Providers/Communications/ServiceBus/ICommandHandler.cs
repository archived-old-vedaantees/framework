using System.Threading.Tasks;

namespace Vedaantees.Framework.Providers.Communications.ServiceBus
{
    public interface ICommandHandler<T> where T : Command
    {
        Task Handle(T command);
    }
}