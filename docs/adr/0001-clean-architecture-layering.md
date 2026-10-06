# ADR-0001: Clean Architecture layering with an inward dependency rule

- **Status:** accepted
- **Date:** 2026-09-10
- **Applies to:** everything

## Context

GameStore is a catalogue-and-checkout application whose interesting parts are business
rules: what a valid basket is, when stock may be reduced, how a discount is worked out,
which order status transitions are legal. Those rules change for business reasons.

The technical surroundings change for entirely different reasons and on a different
clock: the database, the HTTP framework, the email provider. We do not want a change of
persistence technology to be able to reach into a rule about order cancellation.

The team is comfortable with dependency injection and interface-driven design.

## Considered options

### Option A: A single project, folders for separation

Cheapest to start and perfectly workable for an application of this size. The dependency
rule is a convention, though, and conventions that the compiler cannot check erode. The
first time somebody needs a `DbContext` inside a pricing rule, nothing stops them.

### Option B: Four projects, dependencies pointing inward

Domain, Application, Infrastructure, Presentation as separate assemblies. The dependency
rule becomes a compile error rather than a code review comment. Costs three extra
projects and a certain amount of ceremony for small changes.

### Option C: Vertical slices, one module per feature

Attractive when features are genuinely independent. Ours are not: Game, Basket and Order
are deeply entangled, and slicing would mostly produce shared kernels.

## Decision

Option B. Four projects:

```
Presentation  ->  Application  ->  Domain
Infrastructure ->  Application  ->  Domain
```

- **Domain** references nothing. No EF Core, no ASP.NET, no DI container.
- **Application** references Domain. It defines the interfaces it needs from the outside
  world (`IUnitOfWork`, `IEmailService`, the repository interfaces) but implements none
  of them.
- **Infrastructure** references Application and Domain, and implements those interfaces.
- **Presentation** references Application and Infrastructure, and wires them together in
  `Program.cs`.

Repository *interfaces* live in the Application layer rather than the Domain layer.
This is arguable — see `src/GameStore.Application/Repositories/README.md` for the
argument we had — and the deciding factor was that no domain rule in this codebase
currently needs to load anything.

## Consequences

- The Domain project has no package references at all, and that is a property worth
  protecting. If a change requires adding one, treat it as a design smell first and a
  packaging problem second.
- Adding a use case touches three or four files across three projects. That friction is
  real, and it is the price of the compile-time guarantee.
- Infrastructure concerns cannot leak inward accidentally; they can only leak by
  someone adding a project reference, which is visible in a diff.
- Anything shared between Application and Presentation must be expressed in Domain terms
  or duplicated. In practice this means endpoints project domain objects into anonymous
  response objects by hand.
