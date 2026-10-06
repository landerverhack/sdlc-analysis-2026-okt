using GameStore.Application.Abstractions;
using GameStore.Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GameStore.Infrastructure.Messaging;

internal sealed class DomainEventPublisher(
  IServiceProvider serviceProvider,
  ILogger<DomainEventPublisher> logger) : IDomainEventPublisher
{
  public async Task PublishAsync<TDomainEvent>(
    TDomainEvent domainEvent,
    CancellationToken cancellationToken = default)
    where TDomainEvent : IDomainEvent
  {
    var handlers = serviceProvider.GetServices<IDomainEventHandler<TDomainEvent>>().ToList();

    logger.LogInformation(
      "Domain event published: {EventType} {@DomainEvent}. HandlerCount={HandlerCount}",
      typeof(TDomainEvent).Name,
      domainEvent,
      handlers.Count);

    foreach (var handler in handlers)
    {
      await handler.HandleAsync(domainEvent, cancellationToken);
    }
  }
}
