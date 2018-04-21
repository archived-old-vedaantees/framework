using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Vedaantees.Framework.Types.Users;

namespace Vedaantees.Framework.Providers.Users
{
    public static class UserExtensions
    {
        public static string GetSubject(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(p => p.Type == JwtClaimTypes.Subject)?.Value;
        }

        public static string GetEmail(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(p => p.Type == JwtClaimTypes.Email)?.Value;
        }

        public static string GetFamilyName(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(p => p.Type == JwtClaimTypes.FamilyName)?.Value;
        }

        public static string GetGivenName(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(p => p.Type == JwtClaimTypes.GivenName)?.Value;
        }

        public static UserIdentity GetUserIdentity(this ClaimsPrincipal user)
        {
            var subClaim = user.Claims.FirstOrDefault(p => p.Type == "sub");

            return new UserIdentity
            {
                Username = subClaim == null ? "ANONYMOUS" : subClaim.Value
            };
        }
    }
}
