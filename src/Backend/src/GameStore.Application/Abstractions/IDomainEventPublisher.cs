using GameStore.Domain.Abstractions;

namespace GameStore.Application.Abstractions;

public interface IDomainEventPublisher
{
  Task PublishAsync<TDomainEvent>(
    TDomainEvent domainEvent,
    CancellationToken cancellationToken = default)
    where TDomainEvent : IDomainEvent;
}
