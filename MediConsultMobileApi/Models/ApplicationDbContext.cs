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
            modelBuilder.Entity<Request>().Property(r => r.created_date).HasDefaultValue(DateTime.Now.ToString("dd-MM-yyyy"));
            modelBuilder.Entity<Request>().Property(r => r.Client_user_id).HasDefaultValue(0);
            modelBuilder.Entity<Request>().Property(r => r.Provider_location_id).HasDefaultValue(0);
            modelBuilder.Entity<Request>().Property(r => r.Status).HasDefaultValue("Pending");
            modelBuilder.Entity<Request>().Property(r => r.Approval_id).HasDefaultValue(null);
            modelBuilder.Entity<Request>().Property(r => r.Is_pharma).HasDefaultValue(0);
            modelBuilder.Entity<Request>().Property(r => r.Is_notified).HasDefaultValue(0);
            modelBuilder.Entity<Request>().Property(r => r.Mc_notified).HasDefaultValue(0);


        }

        public DbSet<Member> Members { get; set; }

        public DbSet<Member_services_with_copayments> member_Services_With_Copayments { get; set; }

        //public DbSet<Login> logins { get; set; }

        public DbSet<MedicalNetwork> medicalNetworks { get; set; }
   
        public DbSet<ProviderData> Providers { get; set; }

        public DbSet<Request> Requests { get; set; }

        public DbSet<ClientBranchMember> clientBranchMembers { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Refund>  Refunds { get; set; }
        public DbSet<Policy>  Policies { get; set; }
        public DbSet<Service>  Services { get; set; }
        public DbSet<ReimbursementType>  ReimbursementTypes { get; set; }
        public DbSet<ClientPriceList>  ClientPriceLists { get; set; }


    }
}
