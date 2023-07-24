

namespace Infrastructure.Interfaces
{
    public interface IAuditService
    {
        T CreateEntity<T>(T entity);
        T UpdateEntity<T>(T entity);
        T DeleteEntity<T>(T entity);
        string UserName { get; }
        string UserId { get; }
        bool IsTenantUser { get; }
        string TenantId { get; }
        string UserType { get; }
    }
}