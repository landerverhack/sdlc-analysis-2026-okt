using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;

namespace GameStore.Application.Repositories;

/// <summary>
/// Repository interface for Game aggregate
/// </summary>
public interface IGameRepository
{
    Task<Game?> GetByIdAsync(GameId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Game>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Game game, CancellationToken cancellationToken = default);
    void Update(Game game);
    Task<bool> ExistsAsync(GameId id, CancellationToken cancellationToken = default);
}
