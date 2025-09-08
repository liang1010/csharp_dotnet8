using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ht_csharp_dotnet8.Entities
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StaffLabourType>(entity =>
            {
                entity.Property(e => e.BodyRate)
                    .HasPrecision(18, 2); // or HasColumnType("decimal(18,2)")

                entity.Property(e => e.FootRate)
                    .HasPrecision(18, 2);

                entity.Property(e => e.GuaranteeIncomeAmount)
                    .HasPrecision(18, 2);

                entity.Property(e => e.PercentageRate)
                    .HasPrecision(5, 2); // example for percentage like 99.99
            });
        }
        public DbSet<Log> Logs { get; set; }
        public DbSet<SystemConfig> SystemConfigs { get; set; }
        public DbSet<Navigation> Navigations { get; set; }
        public DbSet<NavigationRoles> NavigationRoles { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<StaffAccommodation> StaffAccommodations { get; set; }
        public DbSet<StaffBank> StaffBanks { get; set; }
        public DbSet<StaffContact> StaffContacts { get; set; }
        public DbSet<StaffLabourType> StaffLabourTypes { get; set; }
        public DbSet<StaffStatus> StaffStatuss { get; set; }

    }
}
