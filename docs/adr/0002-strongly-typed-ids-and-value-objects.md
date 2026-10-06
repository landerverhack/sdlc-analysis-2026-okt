# ADR-0002: Strongly-typed ids and value objects instead of primitives

- **Status:** accepted
- **Date:** 2026-09-10
- **Applies to:** domain

## Context

Almost every identifier in this system is an integer, and almost every amount of money
is a decimal. That means the compiler will happily accept a `CustomerId` where a
`GameId` was meant, or a price in USD added to a price in EUR.

Both mistakes are silent. Neither shows up in a test unless somebody thought to write
that specific test. Both are the kind of defect that reaches production and is then
expensive to find, because the symptom appears far away from the cause.

## Considered options

### Option A: Bare primitives, with naming discipline

`int gameId`, `decimal price`. Zero ceremony. Relies entirely on parameter names, which
the compiler does not check and refactoring tools happily scramble.

### Option B: Strongly-typed ids and value objects

`record struct GameId(int Value)`, `record Money(decimal Amount, CurrencyName Currency)`.
The compiler rejects the mix-up. Costs an EF Core value converter per type and a certain
amount of `.Value` noise at the boundaries.

### Option C: A general-purpose library for this

Vogen, StronglyTypedId and similar can generate all of it. Adds a package reference to
the Domain project, which ADR-0001 says we would rather not have, in exchange for
removing perhaps thirty lines of hand-written types.

## Decision

Option B, hand-written.

- Every entity identity is a `record struct NameId(int Value)`.
- Money is always `Money`, never `decimal`. `Money` carries its `CurrencyName`.
- Concepts with validation rules get a value object with the validation in the
  constructor: `EmailAddress` rejects malformed input and lower-cases; `Address` is a
  record of street and city.
- Value objects with no natural null get an explicit `Empty` (see `EmailAddress.Empty`)
  in preference to being nullable.
- Conversion to and from primitives happens at the boundaries: EF Core converters in
  `ApplicationDbContext.ConfigureConventions`, and hand-written mapping in the endpoints.

## Consequences

- A whole class of argument-mix-up bugs becomes a compile error.
- Every new id type needs a matching EF Core converter registered in
  `ConfigureConventions`, and forgetting it produces a confusing runtime error rather
  than a clear one.
- `.Value` appears throughout the Presentation layer. This is noise, and it is the
  visible edge of the boundary, which is where we want the noise to be.
- Domain code reads in domain terms. `Money.Zero` and `EmailAddress.Empty` say what they
  mean in a way that `0m` and `null` do not.
- Serialisation is never automatic. Every id crossing the HTTP boundary is unwrapped by
  hand, which is tedious but keeps the wire format under deliberate control.
