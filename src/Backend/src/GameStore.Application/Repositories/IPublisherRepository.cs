using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;

namespace GameStore.Application.Repositories;

/// <summary>
/// Repository interface for Publisher aggregate
/// </summary>
public interface IPublisherRepository
{
    Task<Publisher?> GetByIdAsync(PublisherId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Publisher>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Publisher publisher, CancellationToken cancellationToken = default);
    void Update(Publisher publisher);
    Task<bool> ExistsAsync(PublisherId id, CancellationToken cancellationToken = default);
}
