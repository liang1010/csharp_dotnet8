using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace ht_csharp_dotnet8.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string role) : base(role) { }

    }
}