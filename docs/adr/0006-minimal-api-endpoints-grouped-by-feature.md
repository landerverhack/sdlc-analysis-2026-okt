# ADR-0006: Minimal API endpoints grouped by feature, no controllers

- **Status:** accepted
- **Date:** 2026-09-10
- **Applies to:** presentation

## Context

The Presentation layer's whole job is to translate HTTP into a use case call and the
result back into JSON. ADR-0005 already put the orchestration in the Application layer,
so there is very little left for a controller to do.

ADR-0002 means that no domain object is directly serialisable — every id is a wrapper
and every price is a `Money`. Something has to unwrap them, deliberately.

## Considered options

### Option A: MVC controllers

Conventional, well understood, good tooling. Brings model binding, filters and an
inheritance hierarchy to a layer that needs none of them, and hides the route table
behind attributes.

### Option B: Minimal APIs, all in `Program.cs`

Shortest path. `Program.cs` becomes several hundred lines and stops being readable at
about the fifth endpoint.

### Option C: Minimal APIs in static extension classes, one per feature

A `MapXxxEndpoints` extension per resource, each creating a route group. `Program.cs`
stays at a dozen lines. Costs one file per feature and a registration line.

## Decision

Option C.

- One `internal static class XxxEndpoints` per resource in `Presentation/Endpoints/`,
  exposing `MapXxxEndpoints(this IEndpointRouteBuilder)`.
- Each creates a route group with its base path and tag:
  `app.MapGroup("/api/games").WithTags("Games")`.
- All of them are registered in `EndpointRegistration.MapApiEndpoints`, which
  `Program.cs` calls once.
- Request bodies are `internal sealed record` types nested in the same file as the
  endpoints that use them. They are wire contracts and they are not shared with any
  other layer.
- Responses are built inline as anonymous objects, mapping domain objects to primitives
  by hand: `id = game.Id.Value`, `price = game.Price.Amount`.
- Endpoints contain no business logic. They parse, wrap primitives into value objects,
  call one use case, and map the result.
- Error handling is a `try`/`catch` per endpoint: `DomainException` and
  `ArgumentException` become 400 with an `{ error }` body; a not-found lookup becomes
  404.

## Consequences

- The route table is readable. Opening one file shows every route for one resource.
- The wire contract is explicit and cannot drift accidentally with a domain rename,
  because the mapping is written out by hand.
- That same hand mapping is repetitive, and it is where the API vocabulary can quietly
  diverge from the glossary. Nothing checks that it has not.
- `try`/`catch` is duplicated in every endpoint. Exception-handling middleware would
  remove it and would also flatten the distinction between the two exception types,
  which is currently made per endpoint. Worth revisiting when the count of endpoints
  makes the duplication painful.
- Anonymous response objects mean there is no type to point at when documenting the API,
  and OpenAPI generation has less to work with.
- There is no versioning strategy. Adding one later means either a new route group
  prefix or a rewrite of these files.
