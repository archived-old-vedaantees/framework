using System.Collections.Generic;

namespace Vedaantees.Framework.Shell.UserContexts
{
    public interface IUserContext
    {
        string ContextId { get; set; }
        List<UserClaim> Claims { get; set; }
    }
}