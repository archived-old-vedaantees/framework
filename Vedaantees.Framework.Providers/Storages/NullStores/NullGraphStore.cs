using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Vedaantees.Framework.Providers.Logging;
using Vedaantees.Framework.Providers.Storages.Graphs;
using Vedaantees.Framework.Types.Results;

namespace Vedaantees.Framework.Providers.Storages.NullStores
{
    public class NullGraphStore : IGraphStore
    {
        private readonly ILogger _logger;

        public NullGraphStore(ILogger logger)
        {
            _logger = logger;
        }

        public MethodResult Add<T>(string label, T entity) where T : class, IEntity<string>
        {
            _logger.Information("Graphstore disabled by configuration or no service installed.");
            return new MethodResult(MethodResultStates.Successful);
        }

        public MethodResult Update<T>(string label, T entity) where T : class, IEntity<string>
        {
            _logger.Information("Graphstore disabled by configuration or no service installed.");
            return new MethodResult(MethodResultStates.Successful);
        }

        public MethodResult Delete<T>(string label, string id) where T : class, IEntity<string>
        {
            _logger.Information("Graphstore disabled by configuration or no service installed.");
            return new MethodResult(MethodResultStates.Successful);
        }

        public MethodResult Execute(string query, IDictionary<string, object> parameters = null)
        {
            _logger.Information("Graphstore disabled by configuration or no service installed.");
            return new MethodResult(MethodResultStates.Successful);
        }

        public MethodResult CreateRelation<TSource, TDestination>(string label, string sourceLabel, string sourceId,
            string destinationLabel, string destinationId) where TSource : IEntity<string> where TDestination : IEntity<string>
        {
            _logger.Information("Graphstore disabled by configuration or no service installed.");
            return new MethodResult(MethodResultStates.Successful);
        }

        public T GetById<T>(string label, string id) where T : class, IEntity<string>, new()
        {
            _logger.Information("Graphstore disabled by configuration or no service installed.");
            return new T();
        }

        public List<T> Get<T>(string label, Expression<Func<T, bool>> expression) where T : class, IEntity<string>, new()
        {
            _logger.Information("Graphstore disabled by configuration or no service installed.");
            return new List<T>();
        }

        public IEnumerable<T> Execute<T>(string query, IDictionary<string, object> parameters = null)
        {
            _logger.Information("Graphstore disabled by configuration or no service installed.");
            return new List<T>();
        }
    }
}
