using GameStore.Application.Abstractions;
using GameStore.Application.Repositories;
using GameStore.Domain.Events;

namespace GameStore.Application.EventHandlers;

public sealed class GamePriceChangedBasketRecalculationHandler(
  IShoppingBasketRepository shoppingBasketRepository,
  IUnitOfWork unitOfWork) : IDomainEventHandler<GamePriceChanged>
{
  public async Task HandleAsync(GamePriceChanged domainEvent, CancellationToken cancellationToken = default)
  {
    var baskets = await shoppingBasketRepository.GetBasketsContainingGameAsync(domainEvent.GameId, cancellationToken);

    foreach (var basket in baskets)
    {
      basket.RecalculateSubtotal();

      await shoppingBasketRepository.UpdateAsync(basket, cancellationToken);
    }

    if (baskets.Count > 0)
      await unitOfWork.CommitAsync(cancellationToken);
  }
}