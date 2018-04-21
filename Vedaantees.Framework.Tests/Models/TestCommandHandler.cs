using System.Threading.Tasks;
using Vedaantees.Framework.Providers.Communications.ServiceBus;

namespace Vedaantees.Framework.Tests.Models
{
    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        public Task Handle(TestCommand command)
        {
            return Task.CompletedTask;
        }
    }
} 