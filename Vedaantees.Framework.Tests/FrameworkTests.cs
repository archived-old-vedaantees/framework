using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Vedaantees.Framework.Utilities;

namespace Vedaantees.Framework.Tests
{
    [TestClass]
    public class FrameworkTests
    {
        [TestMethod]
        public void TestFlatten()
        {
            var json = JsonConvert.SerializeObject(new Person{ Name="abhay", Contacts = new List<Contact>{ new Contact{ Order = 1, Value="860904" } }});
            var util = new JsonExtensions();
            var keyValues = util.Flatten(json);
            var str = util.Unflatten(keyValues);
            Assert.AreEqual(str, json);
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public List<Contact> Contacts { get; set; }
    }

    public class Contact
    {
        public int Order { get; set; }
        public string Value { get; set; }
    }
}