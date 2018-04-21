using System;
using System.Linq;
using System.Linq.Expressions;
using Vedaantees.Framework.Providers.Storages.Sessions;
using Vedaantees.Framework.Types.Results;
using Omu.ValueInjecter;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Vedaantees.Framework.Providers.Storages
{
    public class DocumentStore : Data.IDocumentStore
    {
        private readonly DocumentSessionFactory _documentSessionFactory;
        private readonly IDocumentStore _documentStore;
        private IDocumentSession _documentSession;
        private readonly InvalidOperationException _documentSessionException;

        public DocumentStore(DocumentSessionFactory documentSessionFactory, IDocumentStore documentStore)
        {
            _documentSessionFactory = documentSessionFactory;
            _documentStore = documentStore;
            _documentSessionException = new InvalidOperationException("Document Session is not initialized. Please SetSession('<database-name>')");
        }
        
        public MethodResult SetSession(string database)
        {
            _documentSession = _documentSessionFactory.GetSession(database);
            return new MethodResult(MethodResultStates.Successful);
        }

        public MethodResult Store<T>(T t) where T : class, IEntity<string>, new()
        {
            if (_documentSession == null) throw _documentSessionException;

            if (string.IsNullOrEmpty(t.Id))
            {
                _documentSession.Store(t);
            }
            else
            {
                var tInDb = _documentSession.Load<T>(t.Id);

                if (tInDb != null)
                {
                    tInDb.InjectFrom(t);
                    _documentSession.Store(tInDb);
                }
                else
                    _documentSession.Store(t);
            }

            if(!_documentSessionFactory.IsUnitOfWorkConfigured)
                _documentSession.SaveChanges();
            
            return new MethodResult(MethodResultStates.Successful);
        }
        
        public MethodResult Remove<T>(string id) where T : class, IEntity<string>, new()
        {
            if (_documentSession == null) throw _documentSessionException;

            var entity = _documentSession.Load<T>(id);
            _documentSession.Delete(entity);

            if (!_documentSessionFactory.IsUnitOfWorkConfigured)
                _documentSession.SaveChanges();

            return new MethodResult(MethodResultStates.Successful);
        }

        public T Get<T>(string id) where T : class, IEntity<string>, new()
        {
            if (_documentSession == null) throw _documentSessionException;

            var document = _documentSession.Load<T>(id);

            if(document==null)
                throw new Exception("No document found for given id.");

            return document;
        }

        public IQueryable<T> Find<T>(Expression<Func<T, bool>> expression = null) where T : class, IEntity<string>, new()
        {
            if (_documentSession == null) throw _documentSessionException;
            if (expression == null)
            {
                return _documentSession.Query<T>();
            }
            else
                return _documentSession.Query<T>().Where(expression);
        }

        public T First<T>() where T : class, IEntity<string>, new()
        {
            if (_documentSession == null) throw _documentSessionException;
            return _documentSession.Query<T>().FirstOrDefault();
        }

        public MethodResult CreateDatabase(string databaseName)
        {
            var operation = new GetDatabaseRecordOperation(databaseName);
            if (_documentStore.Maintenance.Server.Send(operation) == null)
            {
                var dbRecord = new DatabaseRecord(databaseName);
                var createDatabaseOperation = new CreateDatabaseOperation(dbRecord);
                _documentStore.Maintenance.Server.Send(createDatabaseOperation);
            }
            return new MethodResult(MethodResultStates.Successful);
        }
    }
}

