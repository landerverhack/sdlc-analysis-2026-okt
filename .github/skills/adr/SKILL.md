---
name: adr
description: "Use when an architectural decision needs recording — comparing approaches, weighing designs, or writing an architecture decision record. Proposes three options, recommends one, and drafts a numbered ADR."
---

**Only if the decision constrains code that does not exist yet.** Most do not — say so and stop. [docs/adr/README.md](../../../docs/adr/README.md) has the test.

1. **Read** [docs/adr/](../../../docs/adr/) and [docs/glossary.md](../../../docs/glossary.md) first. An option that violates an accepted ADR without saying so is not an option, it is a mistake.

2. **Propose three options.** One must be the smallest change that could work. Each gets:
   - **How it works** — the types, files and layers that change, in glossary terms.
   - **What it costs over the next six months** — what it makes harder, not what it costs to type.
   - **What breaks it** — the requirement change that turns it from a good decision into a bad one. If you cannot name one, you do not understand it well enough to recommend against it.
   - **Which ADRs it upholds, and which it would supersede**, by number.

3. **Recommend one.** Say what separated it from the runner-up, and what fact would change your mind — specific enough that the user could go and check.

4. **Draft it.** Copy [assets/adr-template.md](assets/adr-template.md) to `docs/adr/NNNN-slug.md` at the next free number, `Status: proposed`, and add its row to the index. The template says what each section needs; all three options go under **Considered options**.

5. **Superseding an accepted ADR** means saying so in the new one and changing **only** the status line of the old.

6. **No application code.** You do not accept your own ADR — the user does.
