---
name: spec
description: "Use when writing a feature spec, specifying a feature, turning an interview into requirements, or producing something an implementation can be split from."
---

1. **Read first**: [docs/glossary.md](../../../docs/glossary.md), [docs/adr/](../../../docs/adr/), and the parts of `src/` the feature touches. A requirement that contradicts how the system already works is worse than a missing one. Cite `file:line` wherever the spec depends on current behaviour.

2. **Write `docs/specs/NNN-slug/spec.md`** from [assets/spec-template.md](assets/spec-template.md), taking the next free number. Ask for the slug if it is not obvious. The six sections are fixed.

3. **Do not invent requirements.** Whatever the repository and the conversation do not settle goes under Open questions. A plausible guess written as a requirement is worse than a blank, because nobody goes back to check it.

4. **Every noun in Requirements is a glossary term.** If the feature needs a concept the glossary lacks, do not coin a word — raise it as an open question and note that the glossary needs the entry first.

5. **Link an ADR for each architectural constraint** instead of restating it. If the feature cannot be built within the accepted ADRs, name the one that must be superseded and stop; designing the replacement is the `adr` skill.

6. **Each section has to pass its own test:**
   - **Goal** — one sentence, names who benefits, describes the outcome and not the work.
   - **Scope** — the out-of-scope list may not be empty. If nothing is out of scope the goal is too vague; say so.
   - **Requirements** — numbered, independently checkable, one idea each. Split any containing "and".
   - **Acceptance criteria** — observable behaviour under the requirement number it verifies, including the unhappy paths. Someone who has not read the spec can tell whether each holds.
   - **Constraints** — stack, patterns, performance, security, compatibility.
   - **Open questions** — a table, every row with an owner. Empty claims nothing is undecided, which is almost never true.

7. **No code, not even a type sketch.** A spec containing an implementation gets reviewed as an implementation.

8. **Report which requirements are blocked** by an unanswered open question — those cannot become issues yet — then offer `spec-to-issues`.
