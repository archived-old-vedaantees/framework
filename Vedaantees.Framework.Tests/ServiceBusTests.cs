using Autofac;
using Vedaantees.Framework.Providers.Communications.ServiceBus;
using Vedaantees.Framework.Providers.ServiceBus.Consumers;
using Vedaantees.Framework.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rebus.Handlers;

namespace Vedaantees.Framework.Tests
{
    [TestClass]
    public class ServiceBusTests
    {
        [TestMethod]
        public void CheckMassTransitWorks()
        {
            //var busControl = MassTransit.Bus
            //           .Factory
            //           .CreateUsingRabbitMq(cfg =>
            //           {
            //               var host = cfg.Host(new Uri("rabbitmq://localhost"), h => { h.Username("guest"); h.Password("guest"); }); //configuration.ServiceBus.EndpointAddress
            //                cfg.ReceiveEndpoint(host,"Test-Commands-2", ec =>
            //                {
            //                    ec.Handler<TestCommand>(ct =>
            //                    {
            //                        Assert.AreEqual("Received","Received");
            //                        return Task.CompletedTask;
            //                    });
            //                });
            //            });

            //busControl.Start();
            //busControl.GetSendEndpoint(new Uri("rabbitmq://localhost/Test-Commands-2")).Result.Send(new TestCommand()).Wait(500);
        }
        
        public void CommandHandlersCanBeResolvedAsConsumers()
        {
            var container = MockBuilder.BuildContainer();
            var consumer = (CommandHandlers<TestCommand,ICommandHandler<TestCommand>>) container.Resolve<IHandleMessages<TestCommand>>();
            Assert.AreNotEqual(consumer,null);
        }
    }
}