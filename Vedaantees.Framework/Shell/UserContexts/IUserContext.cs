using System.Collections.Generic;
using Vedaantees.Framework.Types.People;

namespace Vedaantees.Framework.Shell.UserContexts
{
    public interface IUserContext
    {
        string ContextId { get; set; }
        List<UserClaim> Claims { get; }
    }
}