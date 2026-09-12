using System.ComponentModel.DataAnnotations;

namespace MultiTenantSaas.Models
{
    // Example tenant-scoped resource. Replace/extend with your real domain entities.
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
