---
name: Infrastructure layer
description: Rules for EF Core mapping, repositories and messaging.
applyTo: "src/Backend/src/GameStore.Infrastructure/**/*.cs"
---

# Infrastructure layer

Implements the interfaces the Application layer declares. Nothing here is referenced by
Domain or Application ([ADR-0001](../../docs/adr/0001-clean-architecture-layering.md)).

## EF Core

- SQLite. The schema is created at startup with `EnsureCreatedAsync` — **there are no
  migrations.** Changing the model means the existing `gamestore.db` is stale and must
  be deleted. Say so when you change the model.
- One `IEntityTypeConfiguration<T>` per aggregate in `Configurations/`, picked up by
  `ApplyConfigurationsFromAssembly`.
- Every strongly-typed id needs a value converter registered in
  `ApplicationDbContext.ConfigureConventions`
  ([ADR-0002](../../docs/adr/0002-strongly-typed-ids-and-value-objects.md)). A missing
  one is a confusing runtime failure, not a compile error.
- Aggregates have private setters and private constructors
  ([ADR-0003](../../docs/adr/0003-aggregates-own-their-invariants.md)). Map to backing
  fields where needed. Do not relax an entity's encapsulation to make the mapping
  easier — fix the mapping.
- `Money` maps as an owned type or two columns. It never becomes a bare `decimal` that
  loses its currency — see **Money** in [the glossary](../../docs/glossary.md).

## Repositories

Return domain aggregates, fully loaded enough to satisfy their invariants. Include the
navigation properties the aggregate needs — a `ShoppingBasket` without its lines cannot
recalculate its own subtotal.

## Messaging

`DomainEventPublisher` resolves every `IDomainEventHandler<TEvent>` from the container
and invokes them sequentially, in registration order, in-process
([ADR-0004](../../docs/adr/0004-domain-events-dispatched-after-commit.md)).

New handlers are registered in `DependencyInjection.AddInfrastructure`, even though the
handlers themselves live in the Application project. Registration order is the execution
order — if that matters for what you are adding, it is a design problem worth raising.

`MockEmailService` writes to the log. There is no real email provider, and adding one is
its own decision.
