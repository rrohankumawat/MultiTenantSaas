using System.ComponentModel.DataAnnotations;

namespace MultiTenantSaas.Models
{
    public enum UserRole
    {
        Admin = 0,
        Member = 1
    }

    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        [Required, MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.Member;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
