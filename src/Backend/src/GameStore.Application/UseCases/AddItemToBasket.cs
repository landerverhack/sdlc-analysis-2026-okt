using GameStore.Application.Abstractions;
using GameStore.Application.Repositories;
using GameStore.Domain.Abstractions;
using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;

namespace GameStore.Application.UseCases;

public interface IAddItemToBasketUseCase : IUseCase
{
  Task<ShoppingBasket> AddItem(
      CustomerId customerId,
      GameId gameId,
      int quantity,
      CancellationToken cancellationToken);
}

internal sealed class AddItemToBasketUseCase(
    IShoppingBasketRepository basketRepository,
    ICustomerRepository customerRepository,
    IGameRepository gameRepository,
    IUnitOfWork unitOfWork) : IAddItemToBasketUseCase
{
  public async Task<ShoppingBasket> AddItem(
      CustomerId customerId,
      GameId gameId,
      int quantity,
      CancellationToken cancellationToken)
  {
    if (quantity <= 0)
      throw new DomainException("Quantity must be greater than zero");

    var customer = await customerRepository.GetByIdAsync(customerId, cancellationToken);
    if (customer is null)
      throw new DomainException($"Customer {customerId} not found");

    var game = await gameRepository.GetByIdAsync(gameId, cancellationToken);
    if (game is null)
      throw new DomainException($"Game {gameId} not found");

    var basket = await basketRepository.GetByCustomerIdAsync(customerId, cancellationToken);

    if (basket is null)
    {
      basket = ShoppingBasket.Create(customerId);
      basket.AddItem(game, quantity);
      await basketRepository.AddAsync(basket, cancellationToken);
    }
    else
    {
      basket.AddItem(game, quantity);
      await basketRepository.UpdateAsync(basket, cancellationToken);
    }

    await unitOfWork.CommitAsync(cancellationToken);

    return basket;
  }
}
