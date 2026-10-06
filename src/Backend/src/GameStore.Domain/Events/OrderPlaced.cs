using GameStore.Domain.ValueObjects;

namespace GameStore.Domain.Events;

/// <summary>
/// Integration event published when an order is successfully placed
/// Consumed by OrderPlacedEventHandler module to reduce stock
/// </summary>
/// <param name="OrderId"></param>
/// <param name="CustomerId"></param>
/// <param name="OrderDate"></param>
/// <param name="OrderLines"></param>
public sealed record OrderPlaced(OrderId OrderId, CustomerId CustomerId, DateTime OrderDate, IReadOnlyList<OrderPlaced.OrderLineDto> OrderLines)
  : IDomainEvent
{
  /// <summary>
  /// Order line information for integration event
  /// </summary>
  public sealed record OrderLineDto(GameId GameId, int Quantity);
}


  