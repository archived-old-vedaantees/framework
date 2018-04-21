using Vedaantees.Framework.Providers.Storages;
using Vedaantees.Framework.Providers.Storages.Sessions;
using Vedaantees.Framework.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Vedaantees.Framework.Tests
{
    [TestClass]
    public class DocumentStoreTests
    {
        private readonly DocumentStore _documentStore;

        public DocumentStoreTests()
        {
            var configuration = MockBuilder.BuildConfiguration();

            var documentSessionFactory = new DocumentSessionFactory(new Raven.Client.Documents.DocumentStore
                                        {
                                            Urls = new[] { configuration.DocumentStore.Url } 
                                        }, 
                                        new NullLogger(), 
                                        false);

            _documentStore = new DocumentStore(documentSessionFactory, new Raven.Client.Documents.DocumentStore());
            _documentStore.SetSession("Tests");
        }

        [TestInitialize]
        public void Initialize()
        {
            var session = GetRavenDocumentSession();
            session.Store(new TestDocumentEntity { Id = "Tests/2", Name="Test-DocumentStore-2" });
        }

        private static IDocumentSession GetRavenDocumentSession()
        {
            var ravenDocumentStore = new Raven.Client.Documents.DocumentStore { Urls = new []{ "http://localhost:8080" } };
            ravenDocumentStore.Initialize();
            ravenDocumentStore.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord("Tests")));
            var session = ravenDocumentStore.OpenSession("Tests");
            return session;
        }

        [TestMethod]
        public void StoreTests()
        {
            _documentStore.Store(new TestDocumentEntity { Id = "Tests/1", Name = "Test-DocumentStore"});
            
            var session = GetRavenDocumentSession();
            var entity = session.Load<TestDocumentEntity>("Tests/1");
            Assert.AreEqual(entity.Name, "Test-DocumentStore");
        }

        [TestMethod]
        public void GetTests()
        {
            var entity = _documentStore.Get<TestDocumentEntity>("Tests/2");
            Assert.AreEqual(entity.Name, "Test-DocumentStore-2");
        }

        [TestMethod]
        public void DeleteTests()
        {
            _documentStore.Remove<TestDocumentEntity>("Tests/2");
            
            var session = GetRavenDocumentSession();
            var entity = session.Load<TestDocumentEntity>("Tests/2");
            Assert.AreEqual(entity, null);
        }

        [TestCleanup]
        public void CleanUp()
        {
            var ravenDocumentStore = new Raven.Client.Documents.DocumentStore{ Urls = new []{ "http://localhost:8080" } };
            ravenDocumentStore.Initialize();
            ravenDocumentStore.Maintenance.Server.Send(new DeleteDatabasesOperation("Tests",true));
        }
    }
}