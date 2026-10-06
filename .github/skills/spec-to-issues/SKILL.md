---
name: spec-to-issues
description: "Use when breaking a spec into GitHub issues, creating tickets, planning implementation work, or turning requirements into tasks. Creates them with the gh CLI."
---

**The issue is the prompt: write it so that someone who has read nothing else can start.**

1. **Read** the spec, [docs/glossary.md](../../../docs/glossary.md), and the ADRs the spec links.

2. **Split along layer boundaries** ([ADR-0001](../../../docs/adr/0001-clean-architecture-layering.md)) where the layers genuinely separate. One issue is one sitting. "Implement the feature" is not a split.

3. **Order by risk, not by ease.** The change that could invalidate the others goes first, while there is still time to react. An issue that changes a shared type the others build on goes first, and every dependent issue names it.

4. **No issue may depend on an unanswered open question.** Do not write it — list it at the end as blocked, naming the question and its owner.

5. **Each issue** uses [assets/issue-template.md](assets/issue-template.md) and contains:
   - a one-sentence goal, plus links to the spec and to the constraining ADRs;
   - **acceptance criteria copied verbatim** from the spec, keeping the requirement numbers — wanting to reword one means the spec is wrong, so fix the spec first;
   - the files or folders it is expected to touch, as real paths checked against the repository;
   - an explicit out-of-scope line, naming which issue the excluded work belongs to;
   - how to verify it: the command, the request, the state to inspect. `dotnet build src/Backend/GameStore.slnx` is a start, not an answer.

6. **Show the plan and wait for approval** — title, one-line goal, dependencies, in order. Create nothing before then.

7. **Create with `gh`**, writing each body to a temporary file, since Markdown, backticks and newlines do not survive shell quoting:

   ```bash
   gh label list
   gh issue create --title "..." --body-file <path> --label "..."
   ```

   Use labels that already exist; do not invent one without asking. If `gh` is not authenticated or there is no remote, stop and say so — do not quietly write the issues to a Markdown file instead.

8. **Print the numbers and URLs**, say which to start with and why, and list anything still blocked. **No application code.**
