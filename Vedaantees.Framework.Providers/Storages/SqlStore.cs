using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using Vedaantees.Framework.Providers.Storages.Data;
using Vedaantees.Framework.Types.Results;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;

namespace Vedaantees.Framework.Providers.Storages
{
    public class SqlStore : DbContext, ISqlStore
    {
        private readonly IEnumerable<Type> _entities;
        private readonly bool _isUnitOfWorkConfigured;
        private readonly string _connectionString;
        private readonly bool _disableQueryTracking;

        public SqlStore(IEnumerable<Type> entities, SqlStoreSetting sqlStoreSetting, bool isUnitOfWorkConfigured)
        {
            _entities = entities;
            _isUnitOfWorkConfigured = isUnitOfWorkConfigured;
            _connectionString = sqlStoreSetting.ConnectionString;
            _disableQueryTracking = sqlStoreSetting.DisableQueryTracking;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);

            if (_disableQueryTracking)
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in _entities.Where(p => p.GetCustomAttributes(true).Any(a => a.GetType() == typeof(TableAttribute))))
                modelBuilder.Entity(entity);
            
            base.OnModelCreating(modelBuilder);
        }

        public MethodResult Insert<T>(T t) where T : class, IEntity<long>, new()
        {
            Set<T>().Add(t);

            if(!_isUnitOfWorkConfigured)
                Save();

            return new MethodResult(MethodResultStates.Successful);
        }

        public MethodResult Modify<T>(T t) where T : class, IEntity<long>, new()
        {
            var entity = Get<T>(t.Id);
            entity.InjectFrom(t);
            Set<T>().Update(t);

            if (!_isUnitOfWorkConfigured)
                Save();

            return new MethodResult(MethodResultStates.Successful);
        }

        public MethodResult Delete<T>(long id) where T : class, IEntity<long>, new()
        {
            var response = Get<T>(id);
            Set<T>().Remove(response);

            if (!_isUnitOfWorkConfigured)
                Save();

            return new MethodResult(MethodResultStates.Successful);
        }

        public T Get<T>(long id) where T : class, IEntity<long>, new()
        {
            var entity = Set<T>().FirstOrDefault(p=>p.Id==id);
            if (entity == null)
                throw new Exception("No entity found for given id.");

            return entity;
        }

        public IQueryable<T> Find<T>(Expression<Func<T, bool>> expression) where T : class, IEntity<long>, new()
        {
            return Set<T>().Where(expression).AsQueryable();
        }

        public IQueryable<T> GetAll<T>() where T : class, IEntity<long>, new()
        {
            return Set<T>().AsQueryable();
        }

        public IQueryable<T> Query<T>(string query) where T : class, new()
        {
            return Set<T>().FromSql(query).AsQueryable();
        }

        public void Save()
        {
            if (ChangeTracker.HasChanges())
                SaveChanges(true);
        }
    }
}
