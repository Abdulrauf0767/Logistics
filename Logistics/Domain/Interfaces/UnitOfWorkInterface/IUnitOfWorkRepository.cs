namespace Logistics.Domain.Interfaces.UnitOfWorkInterface
{
    public interface IUnitOfWorkRepository : IDisposable
    {
        // method to conform update
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    }
}
