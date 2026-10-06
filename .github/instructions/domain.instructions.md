---
name: Domain layer
description: Rules for entities, value objects, domain events and domain services.
applyTo: "src/Backend/src/GameStore.Domain/**/*.cs"
---

# Domain layer

This project holds the business rules and nothing else. It has no package references
and it must keep none.

Binding decisions:
[ADR-0002](../../docs/adr/0002-strongly-typed-ids-and-value-objects.md),
[ADR-0003](../../docs/adr/0003-aggregates-own-their-invariants.md).

## Entities

- No public setters. `private set` or get-only.
- State changes through named methods that read as domain language — `Rename`,
  `DecreaseStock`, `ConfirmOrder` — never through property assignment from outside.
- The method validates before it mutates. An aggregate cannot be put into an invalid
  state, so callers never need to check first.
- Collections are `IReadOnlyList<T>` over a private backing list. Mutation is a method
  on the aggregate root.
- Child entities have `internal` constructors so only their aggregate can create them.
- A private parameterless constructor for EF Core where one is needed, commented as
  such.
- Creation goes through a static factory or through the parent that the child requires.

## Value objects

- `record` for reference semantics, `record struct` for identities.
- Validation in the constructor. An instance that exists is valid.
- An explicit `Empty` in preference to nullability, where a natural empty exists.
- `Money` always carries its `CurrencyName`, and nothing converts between currencies
  anywhere. Never introduce a monetary concept without deciding its currency — for
  anything not derived from a Shopping Basket, that is an open question, not an
  assumption. See **Money** in [the glossary](../../docs/glossary.md).

## Domain events

- Records in `Events/`, named in the **past tense**, implementing `IDomainEvent`.
- They carry value objects and primitives, never entities.
- The Domain declares them. It does not publish them — that is the use case's job
  ([ADR-0004](../../docs/adr/0004-domain-events-dispatched-after-commit.md)).

## Domain services

Only for logic that genuinely belongs to no single entity because it spans several.
Implement `IDomainService`. If the logic fits on an entity, put it on the entity.

## Language

The domain layer is the *authority* for the glossary's vocabulary — a name introduced
here becomes the term everywhere else. Add the glossary entry before adding the type.
