using System;
using Vedaantees.Framework.Providers.Communications.ServiceBus;

namespace Vedaantees.Framework.Tests.Models
{
    public class TestCommand : Command
    {
        public TestCommand()
        {
            DateNow = DateTime.Now;
        }

        public DateTime DateNow { get; set; }
    }
}