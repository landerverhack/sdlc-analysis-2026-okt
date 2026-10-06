using GameStore.Domain.ValueObjects;

namespace GameStore.Domain.Entities;

public sealed class BasketLine : IEntity<BasketLineId>
{
  public BasketLineId Id { get; private set; }
  public GameId GameId { get; private set; }
  public Game Game { get; private set; } = default!;
  public int Quantity { get; private set; }
  public Money LineTotal => new(Game.Price.Amount * Quantity, Game.Price.Currency);

  private BasketLine() { }

  internal BasketLine(Game game, int quantity)
  {
    GameId = game.Id;
    Game = game;
    Quantity = quantity;
  }

  internal void SetQuantity(int quantity)
  {
    if (quantity <= 0)
      throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

    Quantity = quantity;
  }

  internal void IncreaseQuantity(int quantity)
  {
    if (quantity <= 0)
      throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

    Quantity += quantity;
  }
}
