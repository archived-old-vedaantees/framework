using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Vedaantees.Framework.Providers.Storages.Graphs;
using Vedaantees.Framework.Types.Results;
using Neo4jClient;
using Neo4jClient.Cypher;
using Neo4jClient.Transactions;

namespace Vedaantees.Framework.Providers.Storages
{
    public class GraphStore : IGraphStore
    {
        private readonly IGraphClient _graphClient;
        private readonly ITransactionalGraphClient _transactionalGraphClient;
        private readonly bool _isUnitOfWorkConfigured;
        private bool _hasChanged;

        public GraphStore(IGraphClient graphClient, bool isUnitOfWorkConfigured)
        {
            _graphClient = graphClient;
            _transactionalGraphClient = (ITransactionalGraphClient) _graphClient;
            _isUnitOfWorkConfigured = isUnitOfWorkConfigured;
            _hasChanged = false;
        }

        public MethodResult Add<T>(string label, T entity) where T : class, IEntity<string>
        {
            _hasChanged = true;

            if (_isUnitOfWorkConfigured)
                _transactionalGraphClient.Cypher
                                         .Create($"(a:{label}{{ a }})")
                                         .WithParams(entity)
                                         .ExecuteWithoutResults();
            else
                _graphClient.Cypher
                            .Create($"(a:{label}{{ a }})")
                            .WithParam("a", entity)
                            .ExecuteWithoutResults();
            
            return new MethodResult(MethodResultStates.Successful);
        }

        public MethodResult Update<T>(string label, T entity) where T : class, IEntity<string>
        {
            _hasChanged = true;

            if (_isUnitOfWorkConfigured)
                _transactionalGraphClient.Cypher
                                         .Match($"(n:{label})")
                                         .Where((T t) => t.Id == entity.Id)
                                         .Set("n = {entity}")
                                         .WithParam("entity", entity)
                                         .ExecuteWithoutResults();
            else
                _graphClient.Cypher
                            .Match($"(n:{label})")
                            .Where((T n) => n.Id == entity.Id)
                            .Set("n = {entity}")
                            .WithParam("entity", entity)
                            .ExecuteWithoutResults();

            return new MethodResult(MethodResultStates.Successful);
        }

        public MethodResult Delete<T>(string label, string id) where T : class, IEntity<string>
        {
            _hasChanged = true;

            if (_isUnitOfWorkConfigured)
                _transactionalGraphClient.Cypher
                                         .OptionalMatch($"(n:{label})<-[r]-()")
                                         .Where((T n) => n.Id == id)
                                         .Delete("r,n")
                                         .ExecuteWithoutResults();
            else
                _graphClient.Cypher
                            .OptionalMatch($"(n:{label})<-[r]-()")
                            .Where((T n) => n.Id == id)
                            .Delete("r,n")
                            .ExecuteWithoutResults();

            return new MethodResult(MethodResultStates.Successful);
        }

        public T GetById<T>(string label, string id) where T : class, IEntity<string>, new()
        {
            var result = _graphClient.Cypher
                                    .Match($"(n:{label})")
                                    .Where((T n)=> n.Id == id)
                                    .Return(n=>n.As<T>())
                                    .Results
                                    .FirstOrDefault();
            return result;
        }
        
        public List<T> Get<T>(string label, Expression<Func<T, bool>> expression) where T : class, IEntity<string>, new()
        {
            var name  = expression.Parameters[0]?.Name ?? "p";

            var result = _graphClient.Cypher
                                    .Match($"({name}:{label})")
                                    .Where(expression)
                                    .Return(p => p.As<T>())
                                    .Results;

            return result.ToList();
        }

        public IEnumerable<T> Execute<T>(string query, IDictionary<string, object> parameters = null)
        {
            var response = ((IRawGraphClient)_graphClient).ExecuteGetCypherResults<T>(new CypherQuery(query, parameters, CypherResultMode.Projection));
            return response;
        }

        public MethodResult CreateRelation<TSource, TDestination>(string label, string sourceLabel, string sourceId, string destinationLabel, string destinationId) where TSource : IEntity<string> where TDestination : IEntity<string>
        {
            _hasChanged = true;

            if (_isUnitOfWorkConfigured)
                _transactionalGraphClient.Cypher
                                        .Match($"(u1:{sourceLabel})", $"(u2:{destinationLabel})")
                                        .Where((TSource u1) => u1.Id == sourceId)
                                        .AndWhere((TDestination u2) => u2.Id == destinationId)
                                        .Create($"u1-[:{label}]->u2")
                                        .ExecuteWithoutResults();
            else
                _graphClient.Cypher
                            .Match($"(u1:{sourceLabel})", $"(u2:{destinationLabel})")
                            .Where((TSource u1) => u1.Id == sourceId)
                            .AndWhere((TDestination u2) => u2.Id == destinationId)
                            .Create($"(u1)-[:{label}]->(u2)")
                            .ExecuteWithoutResults();

            return new MethodResult(MethodResultStates.Successful);
        }

        public MethodResult Execute(string query,IDictionary<string,object> parameters = null)
        {
            ((IRawGraphClient)_graphClient).ExecuteCypher(new CypherQuery(query, parameters, CypherResultMode.Set));
            return new MethodResult(MethodResultStates.Successful);
        }

        public void Save()
        {
            if(_isUnitOfWorkConfigured && _hasChanged)
                _transactionalGraphClient.Transaction.Commit();
        }
    }
}