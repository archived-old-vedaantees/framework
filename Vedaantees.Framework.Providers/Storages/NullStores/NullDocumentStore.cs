using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Vedaantees.Framework.Providers.Logging;
using Vedaantees.Framework.Providers.Storages.Data;
using Vedaantees.Framework.Types.Results;

namespace Vedaantees.Framework.Providers.Storages.NullStores
{
    public class NullDocumentStore : IDocumentStore
    {
        private readonly ILogger _logger;

        public NullDocumentStore(ILogger logger)
        {
            _logger = logger;
        }

        public MethodResult SetSession(string database)
        {
            _logger.Information("Documentstore disabled by configuration or no service installed.");
            return new MethodResult(MethodResultStates.Successful);
        }

        public MethodResult Store<T>(T t) where T : class, IEntity<string>, new()
        {
            _logger.Information("Documentstore disabled by configuration or no service installed.");
            return new MethodResult(MethodResultStates.Successful);
        }

        public MethodResult Remove<T>(string id) where T : class, IEntity<string>, new()
        {
            _logger.Information("Documentstore disabled by configuration or no service installed.");
            return new MethodResult(MethodResultStates.Successful);
        }

        public T Get<T>(string id) where T : class, IEntity<string>, new()
        {
            _logger.Information("Documentstore disabled by configuration or no service installed.");
            return new T();
        }

        public T First<T>() where T : class, IEntity<string>, new()
        {
            _logger.Information("Documentstore disabled by configuration or no service installed.");
            return new T();
        }

        public IQueryable<T> Find<T>(Expression<Func<T, bool>> expression = null) where T : class, IEntity<string>, new()
        {
            _logger.Information("Documentstore disabled by configuration or no service installed.");
            return new List<T>().AsQueryable();
        }

        public MethodResult CreateDatabase(string databaseName)
        {
            _logger.Information("Documentstore disabled by configuration or no service installed.");
            return new MethodResult(MethodResultStates.Successful);
        }

        public long GetNextNumericalKey(string collectionName)
        {
            _logger.Information("Documentstore disabled by configuration or no service installed.");
            return 0;
        }

        public string GetNextStringKey(string collectionName)
        {
            _logger.Information("Documentstore disabled by configuration or no service installed.");
            return string.Empty;
        }
    }
}