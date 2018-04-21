using System;
using System.Collections.Generic;
using Vedaantees.Framework.Providers.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Vedaantees.Framework.Providers.Storages.Sessions
{
    /// <summary>
    /// Session factory for document store.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class DocumentSessionFactory : IDisposable
    {
        public bool IsUnitOfWorkConfigured { get; }
        private readonly IDocumentStore _documentStore;
        private readonly ILogger _logger;
        private readonly Dictionary<string, IDocumentSession> _sessions;

        public static int Instance { get; set; }

        public DocumentSessionFactory(IDocumentStore documentStore, ILogger logger, bool isUnitOfWorkConfigured)
        {
            IsUnitOfWorkConfigured = isUnitOfWorkConfigured;
            _documentStore = documentStore;
            _logger = logger;
            _documentStore.Initialize();

            _sessions = new Dictionary<string, IDocumentSession>();
            Instance++;
            logger.Information($"Document Session Factory { Instance }");
        }

        public IDocumentSession GetSession(string databaseName)
        {
            if (_sessions.ContainsKey(databaseName))
                return _sessions[databaseName];


            CreateDatabaseIfNotExists(databaseName);
            _sessions.Add(databaseName, _documentStore.OpenSession(databaseName));
            _logger.Information($"Added {databaseName} to session");
            return _sessions[databaseName];
        }

        private void CreateDatabaseIfNotExists(string databaseName)
        {
            var operation = new GetDatabaseRecordOperation(databaseName);
            if (_documentStore.Maintenance.Server.Send(operation) == null)
            {
                var dbRecord = new DatabaseRecord(databaseName);
                var createDatabaseOperation = new CreateDatabaseOperation(dbRecord);
                _documentStore.Maintenance.Server.Send(createDatabaseOperation);
            }
        }

        public void Save()
        {
            foreach (var documentSession in _sessions)
            {
                _logger.Information($"{documentSession.Key} has {documentSession.Value.Advanced.HasChanges}");

                if (documentSession.Value.Advanced.HasChanges)
                    documentSession.Value.SaveChanges();
            }
        }

        public void Dispose()
        {
            _sessions.Clear();
        }
    }
}