# GameStore — agent instructions

An e-commerce API for a board game store, built with Clean Architecture, with an Angular
client in `src/Frontend/`. This repository is the working environment for an
analysis-and-design exercise: most of the work here is *deciding what to build*, and
only then building it.

| Path | What lives there |
| --- | --- |
| `docs/glossary.md` | The ubiquitous language. Binding. |
| `docs/adr/` | Architecture decision records. Binding. |
| `docs/specs/` | One folder per feature: `NNN-slug/spec.md` |
| `docs/design/` | Accepted UI mockups, kept as documentation |

## Domain language

Use the terms in [docs/glossary.md](../docs/glossary.md) exactly — in code, UI copy, JSON
and docs. Never introduce a synonym for a term listed there. If a concept is missing,
propose an addition to the glossary before writing any code.

## Architecture

Every decision that shapes this codebase is recorded in [docs/adr/](../docs/adr/). Read the
relevant ADR before proposing a change to the pattern it describes. If you believe an
ADR should change, say so and propose a new ADR that supersedes it — do not quietly
deviate. ADRs are immutable once accepted.

Layer-specific rules live in `.github/instructions/` and are applied automatically
based on the files being edited.

## How work flows here

Analysis precedes implementation, and each step produces a written artifact that the
next step consumes. Steps 1–6 are skills in `.github/skills/`:

1. **Interview** — clarify the request until the ambiguity is gone (`/interview`)
2. **Glossary** — name the new concepts before modelling them (`/glossary-audit`)
3. **ADR** — *rarely*: only when a choice constrains code that does not exist yet (`/adr`)
4. **Spec** — write `docs/specs/NNN-slug/spec.md` (`/spec`)
5. **Mockups** — compare UI options, keep the winner in `docs/design/` (`/mockups`)
6. **Issues** — split the spec into independently workable issues (`/spec-to-issues`)
7. **Implement** — one issue at a time, grounded in the spec and the ADRs

During steps 1–6, **do not write application code.** Producing documents, mockups and
issues is the work. If you catch yourself opening a `.cs` file to edit it during
analysis, stop and ask.

## Grounding rules

- Prefer reading the codebase over asking. If a question can be answered by exploring
  `src/`, explore `src/` instead of asking the user.
- Do not invent requirements. Anything you cannot determine from the repository or from
  the user belongs under **Open questions** in the spec.
- Cite your evidence. When you assert how something currently works, reference the file
  and line.

## Build and run

```bash
dotnet build src/Backend/GameStore.slnx
dotnet run --project src/Backend/src/GameStore.Presentation
```

The API listens on `http://localhost:5038`.

After changing C# code, run `dotnet build src/Backend/GameStore.slnx --no-restore`, read
every compiler and analyzer diagnostic, and fix the ones your change introduced. Do not
suppress a rule to make the build pass. Repeat after the final edit.

There is no test project, backend or frontend. Do not add one, or a test framework, as a
side effect of another change. If work needs tests, raise it as an open question — how
to test is an ADR.
