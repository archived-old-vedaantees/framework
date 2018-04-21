using System;
using System.Collections.Generic;
using System.Linq;
using Vedaantees.Framework.Providers.Storages.Sessions;
using Vedaantees.Framework.Providers.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vedaantees.Framework.Providers.Mailing;
using Vedaantees.Framework.Shell.UserContexts;
using Vedaantees.Framework.Tests.Models;
using Newtonsoft.Json;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using Vedaantees.Framework.Types.Users;
using DocumentStore = Vedaantees.Framework.Providers.Storages.DocumentStore;

namespace Vedaantees.Framework.Tests
{
    [TestClass]
    public class UserContextTests
    {
        private IUserContextService _userContextService;
        private DocumentSessionFactory _documentSessionFactory;
        private string _testUserId;
        private Guid _contextId;

        [TestInitialize]
        public void Initialize()
        {
            var documentStore = GetSession(out IDocumentSession documentSession);
            _contextId = Guid.NewGuid();

            var testContext2 = new TestContext2
            {
                ContextId  = _contextId,
                AssertContent = "My-Init-Test-Content"
            };

            testContext2.Claims.Add(new UserClaim { Type = "CustomType", Value = "CustomValue" });

            var entity = new User
            {
                Username =  "TestUser",
                Id = null,
                Contexts = new List<Context> {
                                                new Context {
                                                                Id = testContext2.ContextId.ToString(),
                                                                Claims = testContext2.Claims,
                                                                Content = JsonConvert.SerializeObject(testContext2),
                                                                Type = testContext2.GetType().FullName }
                                                            }
                                             };

            documentSession.Store(entity);
            documentSession.SaveChanges();

            _testUserId = entity.Id;
            _documentSessionFactory = new DocumentSessionFactory(documentStore, new NullLogger(), false);
            _userContextService = new UserContextService(new DocumentStore(_documentSessionFactory, new Raven.Client.Documents.DocumentStore()));
        }

        private Raven.Client.Documents.DocumentStore GetSession(out IDocumentSession documentSession)
        {
            var documentStore = new Raven.Client.Documents.DocumentStore { Urls = new []{ MockBuilder.BuildConfiguration().DocumentStore.Url } };
            documentStore.Initialize();
            CreateDatabaseIfNotExists(documentStore,"Users");
            documentSession = documentStore.OpenSession("Users");
            return documentStore;
        }

        private void CreateDatabaseIfNotExists(IDocumentStore documentStore, string databaseName)
        {
            var operation = new GetDatabaseRecordOperation(databaseName);
            if (documentStore.Maintenance.Server.Send(operation) == null)
            {
                var dbRecord = new DatabaseRecord(databaseName);
                var createDbOp = new CreateDatabaseOperation(dbRecord);
                documentStore.Maintenance.Server.Send(createDbOp);
            }
        }

        [TestMethod]
        public void StoreDataToContext()
        {
            _userContextService.StoreContext("TestUser", new Models.TestContext(){ AssertContent = "My-Test-Content" });
            _documentSessionFactory.Save();

            GetSession(out IDocumentSession documentSession);
            var contexts = documentSession.Load<User>(_testUserId)?.Contexts;

            var context = contexts?.OfType<Models.TestContext>().FirstOrDefault();
            Assert.AreEqual(context.AssertContent,"My-Test-Content");
        }

        [TestMethod]
        public void UpdateDataContext()
        {
            _userContextService.StoreContext("TestUser", new TestContext2() { AssertContent = "Changed-My-Test-Content" });
            _documentSessionFactory.Save();

            GetSession(out IDocumentSession documentSession);
            var contexts = documentSession.Load<User>(_testUserId)?.Contexts;

            var context = contexts?.OfType<TestContext2>().FirstOrDefault();
            Assert.AreEqual(context.AssertContent, "Changed-My-Test-Content");
        }

        [TestMethod]
        public void RemoveDataContext()
        {
            _userContextService.RemoveContext("TestUser", _contextId.ToString());
            _documentSessionFactory.Save();

            GetSession(out IDocumentSession documentSession);
            var contexts = documentSession.Load<User>(_testUserId)?.Contexts;

            var context = contexts?.OfType<TestContext2>().FirstOrDefault();
            Assert.AreNotEqual(context.AssertContent, "Changed-My-Test-Content");
        }

        [TestMethod]
        public void GetDataContext()
        {
            var context = _userContextService.GetContext<TestContext2>("TestUser").Result;
            Assert.AreEqual(context.AssertContent, "My-Init-Test-Content");
        }

        [TestMethod]
        public void GetContextsByClaim()
        {
            var contexts = _userContextService.QueryContextsByClaim<TestContext2>(new UserClaim("CustomType", "CustomValue"));
            Assert.AreEqual(contexts.Result.Count(),1);
        }

        [TestCleanup]
        public void Cleanup()
        {
            GetSession(out IDocumentSession documentSession);
            var user = documentSession.Load<User>(_testUserId);
            documentSession.Delete(user);
            documentSession.SaveChanges();
        }

        [TestMethod]
        public void TestRazorTemplate()
        {
            var builder = new TemplateBuilder();
            Assert.AreEqual(builder.Build("Hello {{Name}}", new Model { Name = "Earth" }),"Hello Earth");
        }
    }

    public class Model : ITemplateModel
    {
        public string Name { get; set; }
    }
}