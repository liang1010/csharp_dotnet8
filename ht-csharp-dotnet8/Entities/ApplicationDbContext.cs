using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ht_csharp_dotnet8.Entities
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<Log> Logs { get; set; }
        public DbSet<SystemConfig> SystemConfigs { get; set; }
        public DbSet<Navigation> Navigations { get; set; }
        public DbSet<NavigationRoles> NavigationRoles { get; set; }

    }
}
