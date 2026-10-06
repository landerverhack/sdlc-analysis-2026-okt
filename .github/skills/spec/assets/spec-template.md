# <Feature name>

- **Spec:** NNN-slug
- **Status:** draft | agreed | implemented
- **Date:** YYYY-MM-DD

## Goal

The outcome in one sentence, and who benefits.

Not what we will build — what will be true afterwards, and for whom.

## Scope

**In scope**

- ...

**Out of scope**

- ... (and where it helps, why — "deferred to spec NNN", or "the business does not want it")

An empty out-of-scope list means the scope has not been thought about yet.

## Requirements

Numbered statements, each independently checkable. One idea per statement — if it
contains "and", consider splitting it.

Every noun is a term from `docs/glossary.md`.

1. **R1** — ...
2. **R2** — ...
3. **R3** — ...

## Acceptance criteria

Observable behaviour, per requirement. Written so that someone who did not write the
spec can tell whether it holds, without reading the code.

Include the unhappy paths, not only the happy path.

**R1**

- Given ..., when ..., then ...
- Given ..., when ..., then ...

**R2**

- Given ..., when ..., then ...

## Constraints

Stack, patterns, performance, security, compatibility. Link the ADR for each
architectural constraint rather than restating it.

- Follows [ADR-NNNN](../../adr/NNNN-....md): ...
- ...

If this feature requires an ADR to be superseded, say so here and link the new ADR.

## Open questions

What is undecided, and **who decides it**. A question with no owner will not get
answered.

| # | Question | Blocks | Owner |
| --- | --- | --- | --- |
| Q1 | ... | R3 | ... |

Anything that could not be determined from the repository or the interview belongs here.
Do not resolve an open question by guessing, and do not implement a requirement that
depends on an unanswered one.
