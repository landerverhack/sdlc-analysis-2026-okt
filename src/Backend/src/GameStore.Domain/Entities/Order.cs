using GameStore.Domain.Customers;
using GameStore.Domain.ValueObjects;

namespace GameStore.Domain.Entities;

/// <summary>
/// Business rules: Once placed, cannot be modified (except status). Order number is unique.
/// </summary>
public sealed class Order : IEntity<OrderId>
{
  private readonly List<OrderLine> _orderLines = [];

  public OrderId Id { get; private set; }
  public string OrderNumber { get; private set; } = string.Empty;
  public CustomerId CustomerId { get; private set; }
  public EmailAddress CustomerEmail { get; private set; } = EmailAddress.Empty;
  public DateTime OrderDate { get; private set; }
  public OrderStatus Status { get; private set; }
  public IReadOnlyList<OrderLine> OrderLines => _orderLines.AsReadOnly();
  public Money Subtotal { get; private set; } = Money.Zero;
  public Money Discount { get; private set; } = Money.Zero;
  public Money FinalTotal { get; private set; } = Money.Zero;

  // EF Core constructor
  private Order() { }

  private Order(
      CustomerId customerId,
      EmailAddress customerEmail,
      IEnumerable<OrderLine> orderLines)
  {
    Id = new OrderId(0);
    OrderNumber = GenerateOrderNumber();
    CustomerId = customerId;
    CustomerEmail = customerEmail;
    OrderDate = DateTime.UtcNow;
    Status = OrderStatus.Placed;
    _orderLines.AddRange(orderLines);
    RecalculateSubtotal();
    Discount = new Money(0m, Subtotal.Currency);
    FinalTotal = Subtotal;
  }

  public static Order CreateFromBasket(
      CustomerId customerId,
      EmailAddress customerEmail,
      ShoppingBasket basket)
  {
    if (customerId.Value == 0)
      throw new ArgumentException("Customer ID cannot be zero", nameof(customerId));
    if (customerEmail.IsEmpty())
      throw new ArgumentException("Customer email cannot be empty", nameof(customerEmail));

    var orderLines = basket.Lines.Select(bl =>
        new OrderLine(bl.GameId, bl.Game.Name, bl.Quantity, bl.Game.Price)).ToList();

    if (orderLines.Count == 0)
      throw new InvalidOperationException("Cannot create order with no items");

    return new Order(customerId, customerEmail, orderLines);
  }

  public void ApplyDiscount(Money discount)
  {
    ArgumentNullException.ThrowIfNull(discount);

    if (discount.Currency != Subtotal.Currency)
      throw new DomainException("Discount currency must match order subtotal currency");

    if (discount.Amount < 0)
      throw new DomainException("Discount amount cannot be negative");

    if (discount.Amount > Subtotal.Amount)
      throw new DomainException("Discount amount cannot exceed order subtotal");

    Discount = discount;
    FinalTotal = Subtotal - discount;
  }

  public void ConfirmOrder()
  {
    if (Status != OrderStatus.Placed)
      throw new InvalidOperationException($"Cannot confirm order in status {Status}");

    Status = OrderStatus.Confirmed;
  }

  public void ShipOrder()
  {
    if (Status != OrderStatus.Confirmed)
      throw new InvalidOperationException($"Cannot ship order in status {Status}");

    Status = OrderStatus.Shipped;
  }

  public void CancelOrder()
  {
    if (Status == OrderStatus.Shipped || Status == OrderStatus.Cancelled)
      throw new InvalidOperationException($"Cannot cancel order in status {Status}");

    Status = OrderStatus.Cancelled;
  }

  private void RecalculateSubtotal()
  {
    var subtotalAmount = _orderLines.Sum(l => l.LineTotal.Amount);
    var currency = _orderLines.FirstOrDefault()?.UnitPrice.Currency ?? CurrencyName.EUR;
    Subtotal = new Money(subtotalAmount, currency);
  }

  private static string GenerateOrderNumber()
  {
    // Format: ORD-YYYYMMDD-GUID (first 8 chars)
    var date = DateTime.UtcNow.ToString("yyyyMMdd");
    var uniquePart = Guid.NewGuid().ToString("N")[..8].ToUpper();
    return $"ORD-{date}-{uniquePart}";
  }
}
