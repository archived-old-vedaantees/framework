namespace Vedaantees.Framework.Tests.Models
{
    public class TestGeneric: ITestGenericInterface<ITestInterfaceForContainer>
    {
        public ITestInterfaceForContainer TestLoad { get; set; }
    }
}