using Microsoft.AspNetCore.Identity;

namespace ht_csharp_dotnet8.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
