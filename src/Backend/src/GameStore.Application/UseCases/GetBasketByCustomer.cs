using GameStore.Application.Abstractions;
using GameStore.Application.Repositories;
using GameStore.Domain.Abstractions;
using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;

namespace GameStore.Application.UseCases;

public interface IGetBasketByCustomerUseCase : IUseCase
{
  Task<ShoppingBasket> GetByCustomerId(CustomerId customerId, CancellationToken cancellationToken);
}

internal sealed class GetBasketByCustomerUseCase(IShoppingBasketRepository basketRepository) : IGetBasketByCustomerUseCase
{
  public async Task<ShoppingBasket> GetByCustomerId(CustomerId customerId, CancellationToken cancellationToken)
  {
    var basket = await basketRepository.GetByCustomerIdAsync(customerId, cancellationToken);
    if (basket is null)
      throw new DomainException($"Shopping basket not found for customer {customerId}");

    return basket;
  }
}
