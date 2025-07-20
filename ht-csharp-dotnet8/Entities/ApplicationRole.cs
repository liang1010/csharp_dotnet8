using Microsoft.AspNetCore.Identity;

namespace ht_csharp_dotnet8.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string role) : base(role) { }
        //public virtual ICollection<NAVIGATION_ITEM> NavigationItem { get; set; }

    }
}