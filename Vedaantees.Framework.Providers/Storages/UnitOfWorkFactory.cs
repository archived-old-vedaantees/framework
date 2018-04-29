using System;
using System.Collections.Generic;
using System.Transactions;
using Vedaantees.Framework.Providers.Storages.Sessions;
using Neo4jClient;
using Rebus.Pipeline;
using Vedaantees.Framework.Providers.Logging;
using Vedaantees.Framework.Providers.Communications.ServiceBus;
using Vedaantees.Framework.Providers.Storages.Data;
using IDocumentStore = Raven.Client.Documents.IDocumentStore;

namespace Vedaantees.Framework.Providers.Storages
{
    public class UnitOfWorkFactory
    {
        private readonly IDocumentStore _documentStore;
        private readonly IEnumerable<Type> _entities;
        private readonly SqlStoreSetting _sqlStoreSetting;
        private readonly ILogger _logger;
        private readonly IGraphClientFactory _graphClientFactory;
        
        public UnitOfWorkFactory(IEnumerable<Type> entities, ILogger logger, IDocumentStore documentStore, SqlStoreSetting sqlStoreSetting, IGraphClientFactory graphClientFactory)
        {
            _documentStore = documentStore;
            _entities = entities;
            _sqlStoreSetting = sqlStoreSetting;
            _logger = logger;
            _graphClientFactory = graphClientFactory;
        }
        
        public UnitOfWork Create(IMessageContext messageContext)
        {
            var uow = new UnitOfWork();

            if(_documentStore!=null)
                uow.SetStore(new DocumentSessionFactory(_documentStore, _logger, true));

            if (_sqlStoreSetting != null)
                uow.SetStore(new SqlStore(_entities, _sqlStoreSetting, true));

            if (_graphClientFactory != null)
                uow.SetStore(new GraphStore(_graphClientFactory.Create(), true));

            messageContext.TransactionContext.Items.GetOrAdd("UnitOfWork", uow);
            return uow;
        }
        
        public void Cleanup(IMessageContext messageContext, object arg)
        {
            var unitOfWork = messageContext.TransactionContext.Items["UnitOfWork"] as UnitOfWork;
            unitOfWork?.DocumentSessionFactory.Dispose();
            unitOfWork?.SqlStore.Dispose();
        }

        public void Commit(IMessageContext messageContext, object arg)
        {

            var command = messageContext.Message.Body as Command;

            using (var scope = new TransactionScope())
            {
                try
                {
                    var unitOfWork = messageContext.TransactionContext.Items["UnitOfWork"] as UnitOfWork;
                    unitOfWork?.DocumentSessionFactory?.Save();
                    unitOfWork?.SqlStore?.Save();
                    unitOfWork?.GraphStore?.Save();
                    scope.Complete();

                    _logger.Information("Commited: {0}", command?.RequestId);
                }
                catch (Exception exception)
                {
                    _logger.Error("Error: {0} - {1}", command?.RequestId, exception);
                    scope.Dispose();
                    throw;
                }
            }
        }
    }
}