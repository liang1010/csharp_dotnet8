using ht_csharp_dotnet8.Attributes;
using System.Security.Claims;

namespace ht_csharp_dotnet8.Helpers
{
    public interface IUserHelper
    {
        string GetRolesFromJwtToken();
    }
    [ServiceDependencies]
    public class UserHelper : IUserHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetRolesFromJwtToken()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null || context.User == null)
            {
                return string.Empty;
            }

            // Find claims with the type representing roles
            var roleClaims = context.User.Claims.Where(c => c.Type == ClaimTypes.Role);

            // Extract role values
            var roles = roleClaims.Select(c => c.Value).FirstOrDefault();

            return roles;
        }
    }
}
