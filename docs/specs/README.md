# Specs

One folder per feature: `NNN-slug/spec.md`, numbered in the order the work started.

```
docs/specs/
  001-rewards/
    spec.md
    mockups/            (optional — options that were compared, if worth keeping)
```

Numbers are never reused and never renumbered. A spec that is abandoned keeps its
number and gets `Status: abandoned`.

## What a spec is for

A spec exists so that implementation can be split up and handed off without the person
doing the work having to reconstruct the reasoning. Every GitHub issue for a feature
links back to its spec, and copies its acceptance criteria verbatim.

The six sections are fixed. The template lives with the skill that fills it in:
[`.github/skills/spec/assets/spec-template.md`](../../.github/skills/spec/assets/spec-template.md).

| Section | Test of whether it is finished |
| --- | --- |
| **Goal** | One sentence, names a beneficiary |
| **Scope** | The out-of-scope list is not empty |
| **Requirements** | Numbered, and each one is independently checkable |
| **Acceptance criteria** | Observable behaviour, traceable to a requirement number |
| **Constraints** | Each architectural constraint links an ADR instead of restating it |
| **Open questions** | Each has an owner |

## The rules that keep specs honest

- **Do not invent requirements.** If the repository and the conversation do not settle
  it, it is an open question. A plausible guess written as a requirement is worse than
  a blank, because nobody will ever check it.
- **Ground every term in the [glossary](../glossary.md).** If the feature needs a new
  concept, add it to the glossary first.
- **Ground every pattern in an [ADR](../adr/).** If the feature cannot be built within
  the existing ADRs, that is a finding: name the ADR that has to be superseded.
- **A spec is written before code, and updated when reality disagrees with it.** A spec
  that no longer matches what was built should be corrected, not quietly abandoned.

Generate one with `/spec`, then split it with `/spec-to-issues`.
