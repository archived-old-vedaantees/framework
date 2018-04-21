using Vedaantees.Framework.Providers.Storages;

namespace Vedaantees.Framework.Types.Users
{
    public class UserIdentity : IEntity<string>
    {
        public string Id { get; set; }
        public string Username { get; set; }
    }
}