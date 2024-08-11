using InstagramCommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace InstagramCommerce.Data
{
    public class InstagramCommerceContext : DbContext
    {
        public InstagramCommerceContext(DbContextOptions<InstagramCommerceContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<SynchronizationLog> SynchronizationLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@$"Server={Environment.MachineName};Database=ConcurrencyDemoDb;Trusted_Connection=True;TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
