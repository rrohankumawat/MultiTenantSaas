using Microsoft.EntityFrameworkCore;
using MultiTenantSaas.Models;
using MultiTenantSaas.Multitenancy;

namespace MultiTenantSaas.Data
{
    public class AppDbContext : DbContext
    {
        private readonly ITenantProvider _tenantProvider;

        public AppDbContext(DbContextOptions<AppDbContext> options, ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        public DbSet<Tenant> Tenants => Set<Tenant>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(t => t.Slug).IsUnique();
            });

            modelBuilder.Entity<User>(b =>
            {
                b.HasIndex(u => new { u.TenantId, u.Email }).IsUnique();

                b.HasOne(u => u.Tenant)
                 .WithMany(t => t.Users)
                 .HasForeignKey(u => u.TenantId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Product>(b =>
            {
                b.HasOne(p => p.Tenant)
                 .WithMany(t => t.Products)
                 .HasForeignKey(p => p.TenantId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasQueryFilter(p => _tenantProvider.TenantId != null && p.TenantId == _tenantProvider.TenantId);
            });
        }
    }
}
