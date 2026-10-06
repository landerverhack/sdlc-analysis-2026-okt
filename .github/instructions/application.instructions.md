---
name: Application layer
description: Rules for use cases, repository interfaces and domain event handlers.
applyTo: "src/Backend/src/GameStore.Application/**/*.cs"
---

# Application layer

This project orchestrates. It does not decide — business rules belong in the Domain
([ADR-0003](../../docs/adr/0003-aggregates-own-their-invariants.md)).

Binding decisions:
[ADR-0004](../../docs/adr/0004-domain-events-dispatched-after-commit.md),
[ADR-0005](../../docs/adr/0005-use-cases-as-single-method-interfaces.md).

## Use cases

One per file, named for the action: `Checkout.cs`, `AddItemToBasket.cs`.

Each file holds, in this order:

```csharp
public interface IThingUseCase : IUseCase
{
  Task<Result> DoThing(SomeId id, CancellationToken cancellationToken);
}

internal sealed class ThingUseCase(IDependency dep, IUnitOfWork unitOfWork) : IThingUseCase
{
  ...
}
```

- Interface public, implementation `internal sealed`.
- Dependencies through the primary constructor.
- `CancellationToken` is the last parameter, always, and is passed on.
- Register it in `DependencyInjection.AddApplication`. Forgetting this fails at startup.

The shape of the body is: load aggregates through repositories, call domain methods,
commit **once** through `IUnitOfWork`, then publish events. If you find yourself writing
an `if` about a business rule, it belongs on the entity instead.

## Domain events

Publish **after** `CommitAsync`, never before. Nothing may observe an event for a
transaction that did not commit.

Handlers implement `IDomainEventHandler<TEvent>`, live in `EventHandlers/`, and are
registered in `Infrastructure/DependencyInjection.cs` — not here. Several handlers may
subscribe to one event; they run in-process, inside the request.

There is no retry and no outbox. A handler that throws leaves the committed change in
place and its consequence undone. If that is unacceptable for what you are adding, say
so rather than building a private retry.

## Repository interfaces

Declared here, implemented in Infrastructure. They return domain aggregates, never
DTOs, and never `IQueryable` or any other EF Core type.

## Boundaries

No EF Core, no `DbContext`, no ASP.NET types in this project. If you need something from
the outside world, declare an interface in `Abstractions/` and let Infrastructure
implement it.
