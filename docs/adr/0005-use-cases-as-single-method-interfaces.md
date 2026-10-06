# ADR-0005: Use cases are single-method interfaces, one per file

- **Status:** accepted
- **Date:** 2026-09-10
- **Applies to:** application

## Context

The Application layer needs a unit of organisation. The default in .NET is a service
class per aggregate — `OrderService`, `GameService` — which starts tidy and becomes a
grab bag: twelve methods, nine dependencies, and every consumer taking all of them to
use one.

We want the answer to "what can a user actually do in this system?" to be visible in the
folder listing.

## Considered options

### Option A: A service class per aggregate

Familiar. Fewer files. Dependencies accumulate at the class level, so every caller
depends on everything the class needs, and the file grows until nobody reads it top to
bottom.

### Option B: MediatR-style request and handler objects

One request record and one handler per operation, dispatched through a mediator. Good
separation, and pipeline behaviours give a clean place for cross-cutting concerns.
Costs a package dependency, indirection at every call site, and a stack trace that goes
through the mediator.

### Option C: One interface with one method per use case

`ICheckoutUseCase.CheckoutBasket(...)`. Direct injection, direct call. No mediator, no
package. More interfaces, and each one needs a DI registration.

## Decision

Option C.

- One use case per file in `Application/UseCases/`, named for the action: `Checkout.cs`,
  `AddItemToBasket.cs`, `GetGameById.cs`.
- Each file holds the `public interface I<Name>UseCase : IUseCase` and the
  `internal sealed class <Name>UseCase` that implements it, in that order.
- The interface is public; the implementation is `internal sealed`. Consumers can only
  depend on the interface.
- Dependencies come in through a primary constructor.
- `IUseCase` is a marker with no members. It exists to make the set discoverable and to
  allow a future convention-based registration.
- The use case orchestrates and does not decide: load aggregates through repositories,
  call domain methods, commit once through `IUnitOfWork`, publish events. Business rules
  belong in the Domain (ADR-0003).
- Registration is explicit in `Application/DependencyInjection.cs`.

## Consequences

- The `UseCases` folder is a readable inventory of what the system does.
- A caller depends on exactly the one operation it uses, which keeps test doubles small.
- Many small files, and one DI line per use case. Forgetting the line is a startup-time
  failure, which is at least early.
- There is no pipeline, so cross-cutting concerns — logging, validation, transactions,
  authorisation — have nowhere central to live. Today they are absent. When the first
  one is genuinely needed, that is the moment to revisit this ADR rather than to
  hand-roll a decorator per use case.
- Use cases cannot easily compose. One calling another means injecting the other's
  interface, which works but blurs the transaction boundary, since each use case expects
  to own its own commit.
