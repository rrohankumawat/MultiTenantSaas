namespace MultiTenantSaas.Multitenancy
{
    public class TenantProvider : ITenantProvider
    {
        public Guid? TenantId { get; private set; }

        public void SetTenant(Guid tenantId)
        {
            TenantId = tenantId;
        }
    }
}
