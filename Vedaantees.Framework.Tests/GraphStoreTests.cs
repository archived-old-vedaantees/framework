using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Vedaantees.Framework.Providers.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo4jClient;
using Neo4j.Driver.V1;
using Vedaantees.Framework.Tests.Models;

namespace Vedaantees.Framework.Tests
{
    [TestClass]
    public class GraphStoreTests
    {
        private readonly GraphStore _graphStore;

        public GraphStoreTests()
        {
            var configuration = MockBuilder.BuildConfiguration();
            var graphClientFactory = new GraphClientFactory(NeoServerConfiguration.GetConfiguration(new Uri($"{configuration.GraphStore.Url}/db/data"),configuration.GraphStore.Username, configuration.GraphStore.Password));
            _graphStore = new GraphStore(graphClientFactory.Create(), false);
        }

        [TestInitialize]
        public void Initialize()
        {
            var session = GetSession();
            session.Run("CREATE (:TestGraphEntity { Id:'1', Name:'Test-Name-1' })");
            session.Run("CREATE (:TestGraphEntity { Id:'1000', Name:'Test-Name-1000' })");
        }

        [TestMethod]
        public void Add()
        {
            _graphStore.Add("TestGraphEntity", new TestGraphEntity
                                               {
                                                    Name = "Test-Name-2",
                                                    Date = DateTime.Now.ToLongDateString(),
                                                    Id = "2"
                                               });

            AssertResultTo("2","Test-Name-2");
        }

        [TestMethod]
        public void CreateRelation()
        {
            _graphStore.CreateRelation<TestGraphEntity, TestGraphEntity>("TestRelationEntity", "TestGraphEntity","1", "TestGraphEntity","1000");
            
            Thread.Sleep(2000);
            var session = GetSession();
            var name = string.Empty;
            var result = session.Run("MATCH (a:TestGraphEntity)-[r:TestRelation]->(b:TestGraphEntity) WHERE a.Id = '1' RETURN b.Name as Name");
            
            foreach (var record in result)
            {
                name = record["Name"].As<string>();
                break;
            }

            Assert.AreEqual(name, "Test-Name-1000");
        }

        [TestMethod]
        public void Update()
        {
            _graphStore.Update("TestGraphEntity", new TestGraphEntity
                                                  {
                                                        Name = "Test-Name-3",
                                                        Date = DateTime.Now.ToLongDateString(),
                                                        Id = "1"
                                                  });

            AssertResultTo("1","Test-Name-3");
        }

        [TestMethod]
        public void Delete()
        {
            _graphStore.Delete<TestGraphEntity>("TestGraphEntity", "1");

            var session = GetSession();
            var result = session.Run("MATCH (a:TestGraphEntity) WHERE a.Id={id} return a.Name as Name", new Dictionary<string, object> { { "id", "1" } });
            Assert.AreEqual(result.ToList().Count,0);
        }

        [TestMethod]
        public void GetById()
        {
            var testGraphEntity = _graphStore.GetById<TestGraphEntity>("TestGraphEntity", "1");

            Assert.AreNotEqual(testGraphEntity, null);
            Assert.AreEqual(testGraphEntity.Name, "Test-Name-1");
        }

        [TestMethod]
        public void GetQuery()
        {
            var entities = _graphStore.Get<TestGraphEntity>("TestGraphEntity", p=>p.Name== "Test-Name-1");

            Assert.AreNotEqual(entities, null);
            Assert.AreNotEqual(entities.Count, 0);
        }

        private ISession GetSession()
        {
            var configuration = MockBuilder.BuildConfiguration();
            return GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic(configuration.GraphStore.Username, configuration.GraphStore.Password)).Session();
        }

        private void AssertResultTo(string id, string matchTo)
        {
            var session = GetSession();
            var result = session.Run("MATCH (a:TestGraphEntity) WHERE a.Id={id} return a.Name as Name", new Dictionary<string, object> { { "id", id } });
            var name = string.Empty;

            foreach (var record in result)
            {
                name = record["Name"].As<string>();
                break;
            }

            Assert.AreEqual(name, matchTo);
        }

        [TestCleanup]
        public void Cleanup()
        {
            var session = GetSession();
            session.Run("MATCH (a:TestGraphEntity) DETACH DELETE a");
            session.Run("MATCH (a:TestRelationEntity) DETACH DELETE a");
        }
        
    }
}