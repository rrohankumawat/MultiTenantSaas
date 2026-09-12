namespace MultiTenantSaas.Multitenancy
{
    public interface ITenantProvider
    {
        Guid? TenantId { get; }
        void SetTenant(Guid tenantId);
    }
}
