using System;
using System.Linq;
using System.Linq.Expressions;
using Vedaantees.Framework.Types.Results;

namespace Vedaantees.Framework.Providers.Storages.Data
{
    public interface ISqlStore
    {
        MethodResult Insert<T>(T t) where T : class, IEntity<long>, new();
        MethodResult Modify<T>(T t) where T : class, IEntity<long>, new();
        MethodResult Delete<T>(long id) where T : class, IEntity<long>, new();

        T Get<T>(long id) where T : class, IEntity<long>, new();
        IQueryable<T> Find<T>(Expression<Func<T, bool>> expression) where T : class, IEntity<long>, new();
        IQueryable<T> GetAll<T>() where T : class, IEntity<long>, new();
        IQueryable<T> Query<T>(string query) where T : class, new();
    }
}