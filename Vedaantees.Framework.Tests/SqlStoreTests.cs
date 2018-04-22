﻿using System;
using System.Collections.Generic;
using System.Linq;
using Vedaantees.Framework.Providers.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using Vedaantees.Framework.Tests.Models;

namespace Vedaantees.Framework.Tests
{
    [TestClass]
    public class SqlStoreTests
    {
        private readonly SqlStore _sqlStore;

        public SqlStoreTests()
        {
            var configuration = MockBuilder.BuildConfiguration();
            _sqlStore = new SqlStore(new List<Type>{ typeof(TestSqlStoreEntity) }, configuration.SqlStore, false);
        }
        
        [TestInitialize]
        public void Initialize()
        {
            var createTable = @"CREATE TABLE IF NOT EXISTS tests(Id BIGINT PRIMARY KEY, Name TEXT);";
            var insert = @"INSERT INTO tests(Id, Name) VALUES(1,'Test-AutoGenerated')";
            ExecuteQuery(createTable);
            ExecuteQuery(insert);
        }

        [TestMethod]
        public void TestInsert()
        {
            var testEntity = new TestSqlStoreEntity { Id = 2, Name = "Test Id"};
            _sqlStore.Insert(testEntity);
            
            var getEntity = _sqlStore.Get<TestSqlStoreEntity>(2);
            Assert.AreNotEqual(getEntity, null);
        }

        [TestMethod]
        public void TestGet()
        {
            var entity = _sqlStore.Get<TestSqlStoreEntity>(1);
            Assert.AreEqual(entity.Name, "Test-AutoGenerated");
        }

        [TestMethod]
        public void TestModify()
        {
            var entity = _sqlStore.Get<TestSqlStoreEntity>(1);
            entity.Name = "Test-AutoGeneratedUpdated";
            _sqlStore.Modify(entity);
            
            var getEntity = _sqlStore.Get<TestSqlStoreEntity>(1);
            Assert.AreEqual(getEntity.Name, "Test-AutoGeneratedUpdated");
        }

        [TestMethod]
        public void TestFind()
        {
            var items = _sqlStore.Find<TestSqlStoreEntity>(p=>p.Name== "Test-AutoGenerated");
            Assert.AreNotEqual(items.ToList().Count, 0);
        }

        [TestMethod]
        public void TestGetAll()
        {
            var items = _sqlStore.GetAll<TestSqlStoreEntity>();
            Assert.AreNotEqual(items.ToList().Count, 0);
        }

        [TestMethod]
        public void TestQuery()
        {
            var items = _sqlStore.Query<TestSqlStoreEntity>("SELECT * FROM Tests");
            Assert.AreNotEqual(items.ToList().Count, 0);
        }

        [TestMethod]
        public void TestDelete()
        {
            TestSqlStoreEntity getEntity = null;
            _sqlStore.Delete<TestSqlStoreEntity>(1);

            try
            {
                getEntity = _sqlStore.Get<TestSqlStoreEntity>(1);
            }
            catch{}

            Assert.AreEqual(getEntity, null);
        }
        
        [TestCleanup]
        public void Cleanup()
        {
            var dropTable = @"DROP TABLE IF EXISTS tests;";
            ExecuteQuery(dropTable);
        }

        private void ExecuteQuery(string query)
        {
            var configuration = MockBuilder.BuildConfiguration();
            var connection = new NpgsqlConnection(configuration.SqlStore.ConnectionString);
            connection.Open();
            var command = new NpgsqlCommand(query, connection);
            command.ExecuteScalar();
            connection.Close();
        }
    }
}