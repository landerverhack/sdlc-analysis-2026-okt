using GameStore.Application.Abstractions;
using GameStore.Application.Repositories;
using GameStore.Domain.Events;

namespace GameStore.Application.EventHandlers;

public sealed class OrderPlacedStockReductionHandler(
  IGameRepository gameRepository,
  IUnitOfWork unitOfWork) : IDomainEventHandler<OrderPlaced>
{
  public async Task HandleAsync(OrderPlaced domainEvent, CancellationToken cancellationToken = default)
  {
    foreach (var line in domainEvent.OrderLines)
    {
      var game = await gameRepository.GetByIdAsync(line.GameId, cancellationToken);
      if (game is null)
        continue;

      game.DecreaseStock(line.Quantity);
      gameRepository.Update(game);
    }

    await unitOfWork.CommitAsync(cancellationToken);
  }
}
