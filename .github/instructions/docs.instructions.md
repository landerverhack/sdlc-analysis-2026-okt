---
name: Documentation
description: Rules for writing glossary entries, ADRs and specs.
applyTo: "docs/**/*.md"
---

# Documentation

These files are read by people and treated as binding by the coding agent. Write them
so that both get the same answer.

## Glossary

[docs/glossary.md](../../docs/glossary.md) is the authority on vocabulary. The domain
layer is the authority on the glossary.

- A term is only real once it has an entry **and** a code home.
- Adding a term happens before the code that uses it.
- Renaming a term means renaming it everywhere, in one change.
- Findings from an audit go in **Under review** and stay there until resolved. Nothing
  gets implemented against a term in that section.
- Do not edit the catalogue or the "Words we do not use" table as a side effect of
  another change.

## ADRs

[docs/adr/](../../docs/adr/) — one numbered file per decision, **immutable once
accepted**. The `adr` skill and its template say how to write one.

- Never edit an accepted ADR except to change its status line to
  `superseded by ADR-NNNN`.
- New ADR: next free number, `Status: proposed`, add a row to the index in
  [docs/adr/README.md](../../docs/adr/README.md).
- When you accept an ADR, check whether an instruction file in `.github/instructions/`
  needs to change with it. Documentation the agent is not pointed at is documentation
  the agent will not read.

## Specs

[docs/specs/](../../docs/specs/) — `NNN-slug/spec.md`. The `spec` skill and its template
say how to write one. Numbers are never reused or renumbered.

## Mockups

[docs/design/](../../docs/design/) holds **only accepted** mockups, and they stay as the
reference the implementation is checked against. Rejected options belong under the spec
that produced them, or nowhere.

## Style

- Link rather than repeat. Two copies of a rule become two different rules.
- Say what something costs, not only what it gives.
