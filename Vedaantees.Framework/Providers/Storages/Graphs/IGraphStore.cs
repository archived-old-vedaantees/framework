using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Vedaantees.Framework.Types.Results;

namespace Vedaantees.Framework.Providers.Storages.Graphs
{
    public interface IGraphStore
    {
        MethodResult Add<T>(string label, T entity) where T : class, IEntity<string>;
        MethodResult Update<T>(string label, T entity) where T : class, IEntity<string>;
        MethodResult Delete<T>(string label, string id) where T : class, IEntity<string>;
        MethodResult Execute(string query, IDictionary<string, object> parameters = null);

        MethodResult CreateRelation<TSource, TDestination>(string label, string sourceLabel, string sourceId,
            string destinationLabel, string destinationId)
            where TSource : IEntity<string>
            where TDestination : IEntity<string>;

        T GetById<T>(string label, string id) where T : class, IEntity<string>, new();
        List<T> Get<T>(string label, Expression<Func<T, bool>> expression) where T : class, IEntity<string>, new();
        IEnumerable<T> Execute<T>(string query, IDictionary<string, object> parameters = null);
    }
}