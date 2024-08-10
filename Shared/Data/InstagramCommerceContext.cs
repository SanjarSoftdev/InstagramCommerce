using InstagramCommerce.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace InstagramCommerce.Shared.Data
{
    public class InstagramCommerceContext : DbContext
    {
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<SynchronizationLog> SynchronizationLogs { get; set; }

        public InstagramCommerceContext(DbContextOptions<InstagramCommerceContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
