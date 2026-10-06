using GameStore.Application.Repositories;
using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Infrastructure.Repositories;

internal sealed class GameRepository(ApplicationDbContext dbContext) : IGameRepository
{
  public Task<Game?> GetByIdAsync(GameId id, CancellationToken cancellationToken = default)
    => dbContext.Games
      .Include(g => g.Publisher)
      .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

  public async Task<IReadOnlyList<Game>> GetAllAsync(CancellationToken cancellationToken = default)
    => await dbContext.Games
      .Include(g => g.Publisher)
      .ToListAsync(cancellationToken);

  public async Task AddAsync(Game game, CancellationToken cancellationToken = default)
    => await dbContext.Games.AddAsync(game, cancellationToken);

  public void Update(Game game)
    => dbContext.Games.Update(game);

  public Task<bool> ExistsAsync(GameId id, CancellationToken cancellationToken = default)
    => dbContext.Games.AnyAsync(g => g.Id == id, cancellationToken);
}
