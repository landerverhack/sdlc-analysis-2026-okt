---
name: interview
description: "Use when a feature request, change or idea is not yet specified enough to build — including 'grill me', 'interview me', 'help me work out what I need', or 'I want to add X'. Asks one question at a time, writes no code, and points at the adr skill when an architectural decision surfaces."
---

Interview me relentlessly about every aspect of this plan until we reach a shared 
understanding.

1. If a question can be answered by exploring the codebase, explore the codebase instead.

2. One question at a time, asked with your ask-the-user tool so the user answers in a prompt. Only fall back to plain chat text if no such tool is available.

3. Propose options and a recommendation if useful. Options are about the business rule, never the implementation.

4. Work the seams: collisions with glossary terms; ADRs the feature strains; cancellation, refund, repetition, and acting on something since deleted; timing and timezone; currency and rounding; who is allowed and what an administrator overrides; how we would know afterwards that it works.

5. Refuse vague answers.

6. Stop when the consequential unknowns run out, or the moment the user says so. Summarise: what was agreed, what is still open and who owns each, which glossary terms and ADRs this collides with.

7. Say whether an architectural choice surfaced — one that constrains code which does not exist yet. Most do not. If one did, name it and recommend the `adr` skill; do not draft the ADR here.

8. Offer the `spec` skill. Do not start writing it.
