using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;

namespace GameStore.Domain.Services;

/// <summary>
/// Example of a domain service that calculates the pricing for an order based on the shopping basket and customer information.
/// It encapsulates the business logic for pricing, which may involve complex rules and calculations that don't belong to any single entity.
/// </summary>
public sealed class OrderPricingService : IOrderPricingService
{
  public PricingResult Calculate(ShoppingBasket basket, Customer customer, DateTime utcNow)
  {
    if (basket.Lines.Count == 0)
      throw new DomainException("Cannot calculate pricing for an empty basket");

    var currencies = basket.Lines
      .Select(line => line.Game.Price.Currency)
      .Distinct()
      .ToList();

    if (currencies.Count != 1)
      throw new DomainException("Basket contains mixed currencies, which is not supported");

    var subtotalAmount = basket.Lines.Sum(line => line.LineTotal.Amount);
    var subtotal = new Money(subtotalAmount, currencies[0]);

    // business rule: VIP customers get a 10% discount on weekends
    var isWeekend = utcNow.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    var discountRate = customer.IsVip && isWeekend ? 0.10m : 0m;
    var discount = new Money(subtotal.Amount * discountRate, currencies[0]);

    var finalTotal = subtotal - discount;

    return new PricingResult(subtotal, discount, finalTotal);
  }
}
