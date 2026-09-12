namespace MultiTenantSaas.Multitenancy
{
    public class TenantResolutionMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantResolutionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantProvider tenantProvider)
        {
            var tenantClaim = context.User?.FindFirst("tenant_id")?.Value;

            if (!string.IsNullOrEmpty(tenantClaim) && Guid.TryParse(tenantClaim, out var tenantId))
            {
                tenantProvider.SetTenant(tenantId);
            }

            await _next(context);
        }
    }
}
