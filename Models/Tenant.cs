using System.ComponentModel.DataAnnotations;

namespace MultiTenantSaas.Models
{
    public class Tenant
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(150)]
        public string CompanyName { get; set; } = string.Empty;

        [Required, MaxLength(80)]
        public string Slug { get; set; } = string.Empty;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
