## Goal

<One sentence. What is true when this issue is closed.>

**Spec:** [NNN-slug](../blob/main/docs/specs/NNN-slug/spec.md) — requirements R<n>, R<n>
**Decisions that constrain this:** [ADR-NNNN](../blob/main/docs/adr/NNNN-....md), [ADR-NNNN](../blob/main/docs/adr/NNNN-....md)
**Language:** [glossary](../blob/main/docs/glossary.md) — use these terms exactly

## Acceptance criteria

Copied verbatim from the spec. Do not reword — if something here is wrong, fix the spec.

**R<n>**

- [ ] Given ..., when ..., then ...
- [ ] Given ..., when ..., then ...

## Expected to touch

- `src/Backend/src/GameStore.<Layer>/<Folder>/<File>.cs` — <what changes>
- `src/Backend/src/GameStore.<Layer>/<Folder>/` — <what is added>

Paths are a guide, not a fence. If the change needs to go somewhere else, say so in a
comment rather than forcing it.

## Out of scope

- <What a reasonable person would otherwise pull in> — that is #<issue>
- <...>

## Depends on

- #<issue> — <why: what it changes that this builds on>

_(Delete this section if nothing blocks it.)_

## How to verify

```bash
dotnet build src/Backend/GameStore.slnx
dotnet run --project src/Backend/src/GameStore.Presentation
```

```http
POST http://localhost:5038/api/<route>
Content-Type: application/json

{ ... }
```

Expected: <the observable result — status code, response body, or the state to inspect
afterwards.>

## Notes

<Anything the implementer needs that is not obvious from the files: an existing pattern
to copy, a trap in the EF Core mapping, a reason the obvious approach is wrong.>
