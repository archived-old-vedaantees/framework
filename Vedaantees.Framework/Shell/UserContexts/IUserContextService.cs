using System.Collections.Generic;
using Vedaantees.Framework.Types.Results;
using Vedaantees.Framework.Types.Users;

namespace Vedaantees.Framework.Shell.UserContexts
{
    public interface IUserContextService
    {
        MethodResult StoreContext<T>(string subjectId, T context, bool allowMultiple = false) where T : IUserContext;
        MethodResult RemoveContext(string subjectId, string contextId);
        MethodResult<T> GetContext<T>(string subjectId) where T : IUserContext;
        MethodResult<T> GetContextByUsername<T>(string username) where T : IUserContext;
        MethodResult<IEnumerable<T>> GetContexts<T>(string subjectId) where T : IUserContext;
        MethodResult<IEnumerable<T>> QueryContextsByClaim<T>(UserClaim userClaim) where T : IUserContext;
    }
}