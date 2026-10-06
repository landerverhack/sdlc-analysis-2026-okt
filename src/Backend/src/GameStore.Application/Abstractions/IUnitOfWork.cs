namespace GameStore.Application.Abstractions;

/// <summary>
/// Unit of Work pattern for managing transactions
/// </summary>
public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
