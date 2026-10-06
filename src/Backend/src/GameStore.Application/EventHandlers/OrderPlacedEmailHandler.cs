using GameStore.Application.Abstractions;
using GameStore.Application.Repositories;
using GameStore.Domain.Events;

namespace GameStore.Application.EventHandlers;

public sealed class OrderPlacedEmailHandler(
  ICustomerRepository customerRepository,
  IEmailService emailService) : IDomainEventHandler<OrderPlaced>
{
  public async Task HandleAsync(OrderPlaced domainEvent, CancellationToken cancellationToken = default)
  {
    var customer = await customerRepository.GetByIdAsync(domainEvent.CustomerId, cancellationToken);
    if (customer is null)
      return;

    var itemCount = domainEvent.OrderLines.Sum(line => line.Quantity);
    var subject = $"Order {domainEvent.OrderId.Value} confirmed";
    var body = $"Hi {customer.FirstName}, your order placed on {domainEvent.OrderDate:u} with {itemCount} item(s) was received.";

    await emailService.SendAsync(customer.Email.Value, subject, body, cancellationToken);
  }
}
