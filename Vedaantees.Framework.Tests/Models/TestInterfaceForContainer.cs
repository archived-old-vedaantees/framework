namespace Vedaantees.Framework.Tests.Models
{
    public class TestInterfaceForContainer : ITestInterfaceForContainer, IOtherTestInterfaceForContainer
    {
        public ITestInterfaceForContainer TestLoad { get; set; }
    }
}