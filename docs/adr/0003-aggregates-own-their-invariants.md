# ADR-0003: Aggregates own their invariants; no public setters

- **Status:** accepted
- **Date:** 2026-09-10
- **Applies to:** domain

## Context

The rules that matter in this system are rules about *state that must always hold*:
stock is never negative, a basket line quantity is always positive, a shipped order is
never cancelled, a discount never exceeds the subtotal it applies to.

A rule enforced at the point of mutation holds always. A rule enforced by the caller
holds until somebody writes a new caller.

There is a second pressure: EF Core needs to materialise these objects from the
database, and it needs some way in.

## Considered options

### Option A: Public setters, validation in the use cases

Anaemic entities, rules in the Application layer. Easy for EF Core, easy to write, and
the rules end up duplicated across every use case that touches the entity — or worse,
present in some and missing in others.

### Option B: Private setters, behaviour methods, validation inside the entity

State changes only through named methods that say what is happening in domain terms.
Requires a constructor EF Core can use, and requires collections to be exposed
read-only.

### Option C: Fully immutable aggregates, every change returns a new instance

Appealing, and a poor fit for EF Core change tracking. Would mean fighting the ORM on
every save.

## Decision

Option B.

- Properties have `private set` (or are get-only). No public setters.
- State changes go through named methods that read as domain language: `Rename`,
  `SetPrice`, `DecreaseStock`, `MoveToNewAddress`, `ConfirmOrder`, `ShipOrder`.
- The method validates before it mutates, and throws `DomainException` for a broken
  business rule or `ArgumentException` for malformed input.
- Collections are exposed as `IReadOnlyList<T>` over a private backing list. Mutating
  the collection is a method on the aggregate root: `AddItem`, `RemoveItem`,
  `UpdateQuantity`.
- Child entities are constructed `internal` so that only the aggregate can create them.
  `BasketLine` is reachable only through `ShoppingBasket`; `OrderLine` only through
  `Order`.
- Aggregates that need one get a private parameterless constructor for EF Core, marked
  as such.
- Creation goes through a static factory (`ShoppingBasket.Create`,
  `Order.CreateFromBasket`) or through the parent (`Publisher.CreateGame`) when the
  parent is required for the child to be valid.

## Consequences

- An invariant lives in exactly one place, and it is the place where the state changes.
- You cannot construct an invalid aggregate, which removes a large category of test.
- EF Core needs private constructors and backing-field configuration, and that mapping
  is easy to get subtly wrong. It is also invisible until something fails to
  materialise.
- Bulk or administrative edits are awkward by construction. When one is needed, it needs
  a named domain method, which is the point but is sometimes a nuisance.
- Two exception types now mean two different things and the endpoints must distinguish
  them: `DomainException` is a broken business rule, `ArgumentException` is malformed
  input. Both currently map to 400, which loses that distinction at the boundary.
