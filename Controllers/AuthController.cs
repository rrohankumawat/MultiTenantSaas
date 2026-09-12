using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenantSaas.Data;
using MultiTenantSaas.DTOs;
using MultiTenantSaas.Models;
using MultiTenantSaas.Services;

namespace MultiTenantSaas.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ITokenService _tokenService;

        public AuthController(AppDbContext db, ITokenService tokenService)
        {
            _db = db;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterTenantRequest request)
        {
            var slug = string.IsNullOrWhiteSpace(request.Slug)
                ? Slugify(request.CompanyName)
                : Slugify(request.Slug);

            var slugTaken = await _db.Tenants.AnyAsync(t => t.Slug == slug);
            if (slugTaken)
            {
                return Conflict(new { message = $"Tenant slug '{slug}' is already taken. Choose a different one." });
            }

            var tenant = new Tenant
            {
                CompanyName = request.CompanyName,
                Slug = slug
            };

            var adminUser = new User
            {
                TenantId = tenant.Id,
                Email = request.AdminEmail.Trim().ToLowerInvariant(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.AdminPassword),
                Role = UserRole.Admin
            };

            _db.Tenants.Add(tenant);
            _db.Users.Add(adminUser);
            await _db.SaveChangesAsync();

            var (token, expiresAtUtc) = _tokenService.GenerateToken(adminUser, tenant.Slug);

            return Ok(new AuthResponse
            {
                Token = token,
                ExpiresAtUtc = expiresAtUtc,
                TenantId = tenant.Id,
                TenantSlug = tenant.Slug,
                UserId = adminUser.Id,
                Email = adminUser.Email,
                Role = adminUser.Role.ToString()
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            var slug = Slugify(request.Slug);
            var email = request.Email.Trim().ToLowerInvariant();

            var user = await _db.Users
                .Include(u => u.Tenant)
                .FirstOrDefaultAsync(u => u.Email == email && u.Tenant!.Slug == slug);

            if (user is null || user.Tenant is null || !user.Tenant.IsActive)
            {
                return Unauthorized(new { message = "Invalid credentials." });
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid credentials." });
            }

            var (token, expiresAtUtc) = _tokenService.GenerateToken(user, user.Tenant.Slug);

            return Ok(new AuthResponse
            {
                Token = token,
                ExpiresAtUtc = expiresAtUtc,
                TenantId = user.TenantId,
                TenantSlug = user.Tenant.Slug,
                UserId = user.Id,
                Email = user.Email,
                Role = user.Role.ToString()
            });
        }

        private static string Slugify(string value)
        {
            return value.Trim().ToLowerInvariant().Replace(" ", "-");
        }
    }
}
