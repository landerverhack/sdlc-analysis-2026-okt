# ADR-0004: Domain events are dispatched by the use case after commit

- **Status:** accepted
- **Date:** 2026-09-10
- **Applies to:** application

## Context

Placing an order has consequences that are not part of placing an order: stock must come
down, a confirmation email must go out. Both are certain to be joined by more over time.

Putting those consequences inside `CheckoutUseCase` makes checkout grow a new dependency
every time the business thinks of something else that should happen at checkout. The use
case then knows about email, which it has no business knowing about.

There is a sequencing question underneath: if the email is sent and the transaction then
rolls back, we have told a customer about an order that does not exist.

## Considered options

### Option A: Call the consequences directly from the use case

Simple, explicit, easy to follow in a debugger. Couples checkout to every downstream
concern and grows without bound.

### Option B: Events raised on the entity, collected and dispatched by the infrastructure

The usual pattern: aggregates hold a list of pending events, and the `DbContext` or an
interceptor dispatches them during or after `SaveChanges`. Keeps the use case clean.
Puts dispatch inside the persistence machinery, where it is hard to see and harder to
debug.

### Option C: Events published explicitly by the use case, after the commit

The use case decides what happened and says so. Dispatch is one visible line at the end
of the method. Nothing is automatic, which means nothing is hidden — and also means it
can be forgotten.

## Decision

Option C.

- Events are records in `Domain/Events/`, named in the past tense, implementing
  `IDomainEvent`: `OrderPlaced`, `GamePriceChanged`.
- An event carries value objects and primitives, not entities. `OrderPlaced` carries an
  `OrderLineDto` of `GameId` and quantity rather than the `OrderLine` itself, so a
  handler cannot navigate the object graph back into the aggregate.
- The use case calls `IUnitOfWork.CommitAsync` **first**, and publishes **afterwards**.
  Nothing observes an event for a transaction that did not commit.
- Handlers implement `IDomainEventHandler<TEvent>` and are registered in
  `Infrastructure/DependencyInjection.cs`. Several handlers may subscribe to one event
  and are invoked in registration order.
- Dispatch is in-process and sequential (`DomainEventPublisher`). There is no queue, no
  retry and no outbox.

## Consequences

- Adding a consequence to checkout means adding a handler and one DI registration. The
  use case does not change.
- Committing before publishing means a handler that fails leaves the system in a state
  where the order exists but its consequence did not happen. `OrderPlacedStockReductionHandler`
  is exactly this risk: stock can silently fail to decrease. We accept it at this scale;
  an outbox is the answer when we cannot.
- Handler ordering is DI registration order. That is implicit coupling, and it will
  surprise somebody eventually.
- Publishing is a manual step in the use case, so a new use case can simply forget to
  publish and nothing will complain.
- Handlers run inside the request. A slow handler is a slow response.
