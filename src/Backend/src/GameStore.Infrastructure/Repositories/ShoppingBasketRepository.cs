using GameStore.Application.Repositories;
using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Infrastructure.Repositories;

internal sealed class ShoppingBasketRepository(ApplicationDbContext dbContext) : IShoppingBasketRepository
{
  public Task<ShoppingBasket?> GetByCustomerIdAsync(CustomerId customerId, CancellationToken cancellationToken = default)
    => dbContext.ShoppingBaskets
      .Include(b => b.Lines)
      .ThenInclude(l => l.Game)
      .FirstOrDefaultAsync(b => b.CustomerId == customerId, cancellationToken);

  public Task<ShoppingBasket?> GetByIdAsync(ShoppingBasketId id, CancellationToken cancellationToken = default)
    => dbContext.ShoppingBaskets
      .Include(b => b.Lines)
      .ThenInclude(l => l.Game)
      .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

  public async Task<IReadOnlyList<ShoppingBasket>> GetBasketsContainingGameAsync(GameId gameId, CancellationToken cancellationToken = default)
    => await dbContext.ShoppingBaskets
      .Include(b => b.Lines)
      .ThenInclude(l => l.Game)
      .Where(b => b.Lines.Any(l => l.GameId == gameId))
      .ToListAsync(cancellationToken);

  public async Task AddAsync(ShoppingBasket basket, CancellationToken cancellationToken = default)
    => await dbContext.ShoppingBaskets.AddAsync(basket, cancellationToken);

  public Task UpdateAsync(ShoppingBasket basket, CancellationToken cancellationToken = default)
  {
    dbContext.ShoppingBaskets.Update(basket);
    return Task.CompletedTask;
  }

  public Task DeleteAsync(ShoppingBasket basket, CancellationToken cancellationToken = default)
  {
    dbContext.ShoppingBaskets.Remove(basket);
    return Task.CompletedTask;
  }
}
