using Vedaantees.Framework.Providers.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Vedaantees.Framework.Tests
{
    [TestClass]
    public class StorageTests
    {
        [TestMethod]
        public void TestGenerateKey()
        {
            var configuration = MockBuilder.BuildConfiguration();
            var documentStore = new Raven.Client.Documents.DocumentStore { Urls = new []{ configuration.DocumentStore.Url }  };
            documentStore.Initialize();
            var generateKey = new GenerateKey(documentStore);
            Assert.IsTrue(generateKey.GetNextNumericalKey("Tests")!=0);
        }

        [TestMethod]
        public void TestHashIds()
        {
            var configuration = MockBuilder.BuildConfiguration();
            var hashId = new HashKeys(configuration.EncryptionSalt);
            var encrypted = hashId.Encrypt(12345);
            var decrypt = hashId.Decrypt(encrypted);
            Assert.AreEqual(decrypt, 12345);
        }
    }
}