# Architecture decision records

One numbered Markdown file per decision. **Immutable once accepted.**

If a decision turns out to be wrong, you do not edit the ADR. You write a new one that
supersedes it, and you change the old one's status line to `superseded by ADR-NNNN`.
That single status edit is the only permitted change to an accepted ADR. The record of
having been wrong is the most useful part of the archive.

## Structure

| Section | Contains |
| --- | --- |
| **Status** | `proposed`, `accepted`, `deprecated` or `superseded by ADR-NNNN` |
| **Context** | The forces and constraints *at the time*. Written in the present tense of that moment, and never updated afterwards. |
| **Considered options** | Including the ones that were rejected, and why they were rejected |
| **Decision** | The option chosen |
| **Consequences** | What it costs. Both the good and the bad — an ADR with only upsides in this section is not finished. |

The template lives with the skill that fills it in:
[`.github/skills/adr/assets/adr-template.md`](../../.github/skills/adr/assets/adr-template.md).
Copy it from there — there is deliberately no second copy in this folder to drift out
of step with it.

## Enforcement

ADRs are documentation, not configuration: nothing loads them automatically. The rules
they imply are restated as globbed instruction files in
[`.github/instructions/`](../../.github/instructions/), which the coding agent applies
based on the files being edited. Each instruction file links back to the ADRs that
justify it.

When you accept a new ADR, check whether an instruction file needs to change with it.

## Index

| # | Decision | Status | Applies to |
| --- | --- | --- | --- |
| [0001](0001-clean-architecture-layering.md) | Clean Architecture layering with an inward dependency rule | accepted | everything |
| [0002](0002-strongly-typed-ids-and-value-objects.md) | Strongly-typed ids and value objects instead of primitives | accepted | domain |
| [0003](0003-aggregates-own-their-invariants.md) | Aggregates own their invariants; no public setters | accepted | domain |
| [0004](0004-domain-events-dispatched-after-commit.md) | Domain events are dispatched by the use case after commit | accepted | application |
| [0005](0005-use-cases-as-single-method-interfaces.md) | Use cases are single-method interfaces, one per file | accepted | application |
| [0006](0006-minimal-api-endpoints-grouped-by-feature.md) | Minimal API endpoints grouped by feature, no controllers | accepted | presentation |
| [0007](0007-frontend-stack.md) | Angular standalone components, Tailwind with our own hand-drawn design system, state in signals | accepted | frontend |

## When something is *not* an ADR

Most decisions are not. The test is whether the decision **constrains code that does not
exist yet**. ADR-0005 tells you what to do when you add a use case nobody has written;
ADR-0004 tells you where to put a consequence nobody has thought of. Those earn a
record.

A decision that merely describes the current shape of one class does not — the class
already says it, and a second copy in `docs/` is one more thing to keep in step. That
kind of fact belongs in [the glossary](../glossary.md) if it affects vocabulary, in the
spec that introduced it if it affects one feature, or nowhere.

Rough guide:

| Write an ADR | Do not |
| --- | --- |
| Aggregate boundaries, who owns which invariant | The shape of one field on one entity |
| A pattern other code has to follow | A choice local to one file and cheap to reverse |
| A technology or integration choice | Anything that follows from an ADR already accepted |
| A deliberate rejection of the general case, with a trigger to revisit | Something the code states plainly on its own |

Seven records for a codebase this size is roughly right. An archive that grows with
every feature stops being read, which defeats the point of having one.

## A note on dates

ADRs 0001 to 0006 were recorded retrospectively on 2026-09-10. They document decisions
that were already embodied in the code but had never been written down, which is the
normal state of affairs when you introduce ADRs to an existing codebase. Their Context
sections reconstruct the reasoning; they are honest but not contemporaneous.
