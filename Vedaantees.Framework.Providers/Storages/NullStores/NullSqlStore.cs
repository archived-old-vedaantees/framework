using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Vedaantees.Framework.Providers.Logging;
using Vedaantees.Framework.Providers.Storages.Data;
using Vedaantees.Framework.Types.Results;

namespace Vedaantees.Framework.Providers.Storages.NullStores
{
    public class NullSqlStore : ISqlStore
    {
        private readonly ILogger _logger;

        public NullSqlStore(ILogger logger)
        {
            _logger = logger;
        }

        public MethodResult Insert<T>(T t) where T : class, IEntity<long>, new()
        {
            _logger.Information("SqlStore disabled by configuration or no service installed.");
            return new MethodResult(MethodResultStates.Successful);
        }

        public MethodResult Modify<T>(T t) where T : class, IEntity<long>, new()
        {
            _logger.Information("SqlStore disabled by configuration or no service installed.");
            return new MethodResult(MethodResultStates.Successful);
        }

        public MethodResult Delete<T>(long id) where T : class, IEntity<long>, new()
        {
            _logger.Information("SqlStore disabled by configuration or no service installed.");
            return new MethodResult(MethodResultStates.Successful);
        }

        public T Get<T>(long id) where T : class, IEntity<long>, new()
        {
            _logger.Information("SqlStore disabled by configuration or no service installed.");
            return new T();
        }

        public IQueryable<T> Find<T>(Expression<Func<T, bool>> expression) where T : class, IEntity<long>, new()
        {
            _logger.Information("SqlStore disabled by configuration or no service installed.");
            return new List<T>().AsQueryable();
        }

        public IQueryable<T> GetAll<T>() where T : class, IEntity<long>, new()
        {
            _logger.Information("SqlStore disabled by configuration or no service installed.");
            return new List<T>().AsQueryable();
        }

        public IQueryable<T> Query<T>(string query) where T : class, new()
        {
            _logger.Information("SqlStore disabled by configuration or no service installed.");
            return new List<T>().AsQueryable();
        }
    }
}