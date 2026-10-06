using GameStore.Domain.Abstractions;

namespace GameStore.Application.Abstractions;

public interface IDomainEventHandler<in TDomainEvent>
  where TDomainEvent : IDomainEvent
{
  Task HandleAsync(TDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
