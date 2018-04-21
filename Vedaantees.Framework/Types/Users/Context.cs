using System.Collections.Generic;

namespace Vedaantees.Framework.Types.Users
{
    public class Context
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public List<UserClaim> Claims { get; set; }
    }
}