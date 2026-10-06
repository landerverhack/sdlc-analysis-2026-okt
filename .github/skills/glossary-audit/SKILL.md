---
name: glossary-audit
description: "Use when auditing the domain language for synonyms and collisions, checking naming consistency, extending the ubiquitous language, or before a feature touches layers the glossary has not been checked against."
---

1. **Read [docs/glossary.md](../../../docs/glossary.md).** The domain layer is the authority on meaning. The Application, Infrastructure and Presentation layers have never been checked against it.

2. **Inventory the domain nouns** in the scope given, or `src/` if none was. Type names, property names, method-name fragments, JSON field names in the endpoints, EF Core column names. List the files using each. Ignore technical vocabulary — `Builder`, `Options`, `Context`, `Async`, `Request`.

3. **Sort the findings into three groups.** **Synonyms**: different words, one meaning — the domain layer's word wins. **Collisions**: one word, two meanings, shown with both usages — these matter most, because a reader will never spot them unaided. **Undocumented**: business terms the glossary does not define.

4. **Cite `file:line` for every finding.** Do not report one you have not read — a plausible collision that turns out not to exist wastes more time than a missed one. Rank by consequence, not count: a collision in the API contract outweighs ten private field names.

5. **Append to the "Under review" section** of the glossary, replacing the `_(empty)_` placeholder — a table per group, each finding with a proposed resolution. Propose; do not apply. Renaming is a separate change with its own decision behind it. Do **not** edit the catalogue or the "Words we do not use" table, and change no code.

6. **Say which findings block** the work about to start, and which are merely untidy.
