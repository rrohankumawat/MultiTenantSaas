using System.ComponentModel.DataAnnotations;

namespace MultiTenantSaas.DTOs
{
    public class RegisterTenantRequest
    {
        [Required, MaxLength(150)]
        public string CompanyName { get; set; } = string.Empty;

        // URL-safe unique identifier, e.g. "acme-inc". Auto-generated if omitted.
        [MaxLength(80)]
        public string? Slug { get; set; }

        [Required, EmailAddress]
        public string AdminEmail { get; set; } = string.Empty;

        [Required, MinLength(8)]
        public string AdminPassword { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        [Required]
        public string Slug { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
        public Guid TenantId { get; set; }
        public string TenantSlug { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
