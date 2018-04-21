using System.Collections.Generic;
using Vedaantees.Framework.Providers.Storages;

namespace Vedaantees.Framework.Types.Users
{
    public sealed class User : UserIdentity, IEntity<string>
    {
        public User()
        {
            Contexts = new List<Context>();
        }

        public List<Context> Contexts { get; set; }
    }
}