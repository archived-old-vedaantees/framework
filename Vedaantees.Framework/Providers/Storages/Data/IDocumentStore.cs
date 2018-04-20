using System;
using System.Linq;
using System.Linq.Expressions;
using Vedaantees.Framework.Types.Results;

namespace Vedaantees.Framework.Providers.Storages.Data
{
    public interface IDocumentStore
    {
        MethodResult SetSession(string database);
        MethodResult Store<T>(T t) where T : class, IEntity<string>, new();
        MethodResult Remove<T>(string id) where T : class, IEntity<string>, new();
        T Get<T>(string id) where T : class, IEntity<string>, new();
        T First<T>() where T : class, IEntity<string>, new();
        IQueryable<T> Find<T>(Expression<Func<T, bool>> expression = null) where T : class, IEntity<string>, new();
        MethodResult CreateDatabase(string databaseName);
    }
}