using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;

namespace GameStore.Application.Repositories;

public interface IShoppingBasketRepository
{
    Task<ShoppingBasket?> GetByCustomerIdAsync(CustomerId customerId, CancellationToken cancellationToken = default);
    Task<ShoppingBasket?> GetByIdAsync(ShoppingBasketId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ShoppingBasket>> GetBasketsContainingGameAsync(GameId gameId, CancellationToken cancellationToken = default);
    Task AddAsync(ShoppingBasket basket, CancellationToken cancellationToken = default);
    Task UpdateAsync(ShoppingBasket basket, CancellationToken cancellationToken = default);
    Task DeleteAsync(ShoppingBasket basket, CancellationToken cancellationToken = default);
}
