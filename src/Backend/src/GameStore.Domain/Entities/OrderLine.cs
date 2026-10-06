using GameStore.Domain.ValueObjects;

namespace GameStore.Domain.Entities;

public sealed class OrderLine : IEntity<OrderLineId>
{
  public OrderLineId Id { get; private set; }
  public GameId GameId { get; private set; }
  public string GameName { get; private set; } = string.Empty;
  public int Quantity { get; private set; }
  public Money UnitPrice { get; private set; } = Money.Zero;
  public Money LineTotal => new(UnitPrice.Amount * Quantity, UnitPrice.Currency);

  private OrderLine() { }

  public OrderLine(GameId gameId, string gameName, int quantity, Money unitPrice)
  {
    GameId = gameId;
    GameName = gameName;
    Quantity = quantity;
    UnitPrice = unitPrice;
  }
}
