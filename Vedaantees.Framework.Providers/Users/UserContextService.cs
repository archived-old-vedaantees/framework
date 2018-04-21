using System.Collections.Generic;
using System.Linq;
using Vedaantees.Framework.Providers.Storages.Data;
using Vedaantees.Framework.Shell.UserContexts;
using Vedaantees.Framework.Types.Results;
using Newtonsoft.Json;
using Vedaantees.Framework.Types.Users;

namespace Vedaantees.Framework.Providers.Users
{
    public class UserContextService : IUserContextService
    {
        private readonly IDocumentStore _documentStore;

        public UserContextService(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
            _documentStore.SetSession("Users");
        }

        public MethodResult StoreContext<T>(string subjectId, T context, bool allowMultiple = false) where T : IUserContext
        {
            var user = _documentStore.Get<User>(subjectId);

            if (user == null)
                return new MethodResult<T>(MethodResultStates.UnSuccessful, "User not found");

            if (!allowMultiple)
                foreach (var existingContext in user.Contexts.Where(p=>p.Type==typeof(T).FullName))
                    user.Contexts.Remove(existingContext);

            user.Contexts.Add(new Context {
                                                Id = context.ContextId,
                                                Content = JsonConvert.SerializeObject(context),
                                                Type = typeof(T).FullName,
                                                Claims = context.Claims
                                          });

            return _documentStore.Store(user);
            
        }

        public MethodResult RemoveContext(string subjectId, string contextId)
        {
            var user = _documentStore.Get<User>(subjectId);

            if (user == null)
                return new MethodResult(MethodResultStates.UnSuccessful, "User not found");

            var context = user.Contexts.FirstOrDefault(p => p.Id == contextId);
            if (context == null)
                return new MethodResult(MethodResultStates.UnSuccessful, "User context not found");
            
            user.Contexts.Remove(context);
            return _documentStore.Store(user);
        }

        public MethodResult<T> GetContext<T>(string subjectId) where T : IUserContext
        {
            var user = _documentStore.Get<User>(subjectId);

            if (user==null)
                return new MethodResult<T>(MethodResultStates.UnSuccessful,"User not found");

            var context = user.Contexts.FirstOrDefault(p => p.Type == typeof(T).FullName);

            if (context == null)
                return new MethodResult<T>(MethodResultStates.UnSuccessful, "User context not found");

            return new MethodResult<T>(JsonConvert.DeserializeObject<T>(context.Content));
        }

        public MethodResult<T> GetContextByUsername<T>(string username) where T : IUserContext
        {
            var user = _documentStore.Find<User>(p=>p.Username==username).FirstOrDefault();

            if (user == null)
                return new MethodResult<T>(MethodResultStates.UnSuccessful, "User not found");

            var context = user.Contexts.FirstOrDefault(p => p.Type == typeof(T).FullName);

            if (context == null)
                return new MethodResult<T>(MethodResultStates.UnSuccessful, "User context not found");

            return new MethodResult<T>(JsonConvert.DeserializeObject<T>(context.Content));
        }

        public MethodResult<IEnumerable<T>> GetContexts<T>(string subjectId) where T : IUserContext
        {
            var user = _documentStore.Get<User>(subjectId);

            if (user == null)
                return new MethodResult<IEnumerable<T>>(MethodResultStates.UnSuccessful, "User not found");

            var contexts = user.Contexts.Where(p => p.Type == typeof(T).FullName);
            return new MethodResult<IEnumerable<T>>(contexts.Select(p=> JsonConvert.DeserializeObject<T>(p.Content)));
        }

        public MethodResult<IEnumerable<T>> QueryContextsByClaim<T>(UserClaim userClaim) where T : IUserContext
        {
            var contexts = _documentStore.Find<User>(u=>u.Contexts.Any(uctx=> uctx.Claims
                                                                                  .Any(uc => uc.Type == userClaim.Type && uc.Value == userClaim.Value))).ToList()
                                         .SelectMany(p=>p.Contexts.OfType<T>());
            return new MethodResult<IEnumerable<T>>(contexts);
        }
    }
}