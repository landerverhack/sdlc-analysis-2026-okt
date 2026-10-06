using GameStore.Domain.ValueObjects;

namespace GameStore.Domain.Entities;

/// <summary>
/// Shopping basket entity
/// Business rules: Items can be added/removed, subtotal is calculated automatically
/// </summary>
public sealed class ShoppingBasket : IEntity<ShoppingBasketId>
{
  private readonly List<BasketLine> _lines = [];

  public ShoppingBasketId Id { get; private set; }
  public CustomerId CustomerId { get; private set; }
  public IReadOnlyList<BasketLine> Lines => _lines.AsReadOnly();
  public Money Subtotal { get; private set; } = Money.Zero;

  // EF Core constructor
  private ShoppingBasket() { }

  private ShoppingBasket(CustomerId customerId)
  {
    Id = new ShoppingBasketId(0);
    CustomerId = customerId;
    Subtotal = Money.Zero;
  }

  public static ShoppingBasket Create(CustomerId customerId)
  {
    if (customerId.Value == 0)
      throw new ArgumentException("Customer ID cannot be zero", nameof(customerId));

    return new ShoppingBasket(customerId);
  }

  public void AddItem(Game game, int quantity)
  {
    if (game.Id.Value == 0)
      throw new ArgumentException("Game ID cannot be zero", nameof(game));
    if (quantity <= 0)
      throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

    var existingLine = _lines.FirstOrDefault(l => l.GameId == game.Id);

    if (existingLine is not null)
    {
      existingLine.IncreaseQuantity(quantity);
    }
    else
    {
      _lines.Add(new BasketLine(game, quantity));
    }

    RecalculateSubtotal();
  }

  public void RemoveItem(GameId gameId)
  {
    var line = _lines.FirstOrDefault(l => l.GameId == gameId);
    if (line == null)
      throw new InvalidOperationException($"Game {gameId} not found in basket");

    _lines.Remove(line);

    RecalculateSubtotal();
  }

  public void UpdateQuantity(GameId gameId, int newQuantity)
  {
    if (newQuantity <= 0)
      throw new ArgumentException("Quantity must be greater than zero", nameof(newQuantity));

    var line = _lines.FirstOrDefault(l => l.GameId == gameId);
    if (line == null)
      throw new InvalidOperationException($"Game {gameId} not found in basket");

    line.SetQuantity(newQuantity);

    RecalculateSubtotal();
  }

  public void RecalculateSubtotal()
  {
    var subtotalAmount = _lines.Sum(l => l.LineTotal.Amount);
    var currency = _lines.FirstOrDefault()?.Game.Price.Currency ?? CurrencyName.EUR;
    Subtotal = new Money(subtotalAmount, currency);
  }

  public void Clear()
  {
    _lines.Clear();
    Subtotal = Money.Zero;
  }
}
