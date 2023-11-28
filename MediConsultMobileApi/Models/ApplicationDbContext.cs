using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Member>
        }
        public DbSet<Member> Members { get; set; }
        public DbSet<Member_services_with_copayments> member_Services_With_Copayments { get; set; }
        public DbSet<Login> logins { get; set; }
        public DbSet<MedicalNetwork> medicalNetworks { get; set; }
        public DbSet<ProviderCategory> providerCategories { get; set; }

    }
}
