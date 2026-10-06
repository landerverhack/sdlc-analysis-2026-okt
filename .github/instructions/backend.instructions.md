---
name: Backend
description: Clean Architecture rules that apply across the whole .NET solution.
applyTo: "src/Backend/**/*.cs"
---

# Backend

Binding decisions: [ADR-0001](../../docs/adr/0001-clean-architecture-layering.md).

## The dependency rule

```
Presentation  ->  Application  ->  Domain
Infrastructure ->  Application  ->  Domain
```

Domain references nothing. Never add a package reference to
`GameStore.Domain.csproj` — if a change appears to need one, the design is wrong, and
say so rather than adding it.

Application defines the interfaces it needs from the outside world and implements none
of them. Infrastructure implements them. Presentation wires them together.

## Conventions

- Two-space indentation, matching the existing files.
- Namespaces are file-scoped.
- File-per-type, with the exception of a use case interface and its implementation,
  which share a file (ADR-0005), and endpoint request records, which sit in the file of
  the endpoints that use them (ADR-0006).
- Primary constructors for dependency injection.
- `internal sealed` for implementations; `public` only for the contract a consumer
  actually needs.

## Money and identity

Never a bare `decimal` for an amount — use `Money` (ADR-0002). Never a bare
`int` for an identity — use the strongly-typed id.

## Errors

`DomainException` for a broken business rule. `ArgumentException` for malformed input.
The distinction is load-bearing at the endpoint boundary — do not collapse it.
