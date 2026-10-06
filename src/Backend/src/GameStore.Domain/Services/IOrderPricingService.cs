using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;

namespace GameStore.Domain.Services;

public interface IOrderPricingService : IDomainService
{
  PricingResult Calculate(ShoppingBasket basket, Customer customer, DateTime utcNow);
}

public sealed record PricingResult(Money Subtotal, Money Discount, Money FinalTotal);
