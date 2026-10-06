using GameStore.Application.Abstractions;
using GameStore.Application.Repositories;
using GameStore.Domain.Abstractions;
using GameStore.Domain.Entities;
using GameStore.Domain.Events;
using GameStore.Domain.Services;
using GameStore.Domain.ValueObjects;

namespace GameStore.Application.UseCases;

public interface ICheckoutUseCase : IUseCase
{
  Task CheckoutBasket(CustomerId customerId, CancellationToken cancellationToken);
}

internal sealed class CheckoutUseCase(IShoppingBasketRepository basketRepository,
    IOrderRepository orderRepository,
    ICustomerRepository customerRepository,
    IOrderPricingService orderPricingService,
    IDomainEventPublisher publisher,
    IUnitOfWork unitOfWork) : ICheckoutUseCase
{
  public async Task CheckoutBasket(CustomerId customerId, CancellationToken cancellationToken)
  {
    // Get basket
    var basket = await basketRepository.GetByCustomerIdAsync(customerId, cancellationToken);
    if (basket is null)
      throw new DomainException($"Shopping basket not found for customer {customerId}");

    if (!basket.Lines.Any())
      throw new DomainException("Cannot checkout an empty basket");

    // Validate customer exists
    var customer = await customerRepository.GetByIdAsync(customerId, cancellationToken);
    if (customer is null)
      throw new DomainException($"Customer {customerId} not found");

    var pricing = orderPricingService.Calculate(basket, customer, DateTime.UtcNow);

    // Create order from basket
    var order = Order.CreateFromBasket(
        customerId,
        customer.Email,
        basket);

    if (order.Subtotal != pricing.Subtotal)
      throw new DomainException("Order subtotal does not match calculated pricing subtotal");

    order.ApplyDiscount(pricing.Discount);

    await orderRepository.AddAsync(order, cancellationToken);

    // Clear basket after successful order creation
    basket.Clear();
    await basketRepository.UpdateAsync(basket, cancellationToken);

    await unitOfWork.CommitAsync(cancellationToken);

    // Publish integration event for stock reduction
    var orderLines = order.OrderLines
        .Select(ol => new OrderPlaced.OrderLineDto(ol.GameId, ol.Quantity))
        .ToList();

    await publisher.PublishAsync(new OrderPlaced(
        order.Id,
        order.CustomerId,
        order.OrderDate,
        orderLines),
        cancellationToken);
  }
}