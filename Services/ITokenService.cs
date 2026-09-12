using MultiTenantSaas.Models;

namespace MultiTenantSaas.Services
{
    public interface ITokenService
    {
        (string token, DateTime expiresAtUtc) GenerateToken(User user, string tenantSlug);
    }
}
