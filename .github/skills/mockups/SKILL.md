---
name: mockups
description: "Use when a UI or interaction decision needs comparing — mockups, wireframes, layout options, or 'what should this look like'. Builds three HTML options, opens them in the browser, and lets the human pick the winner."
---

1. **Name the axis the three options disagree on** before building. Three variations on one layout is not a comparison. The options disagree on layout, hierarchy or interaction — never on the look itself, which the design system already fixes.

2. **Build three** from [assets/mockup-shell.html](assets/mockup-shell.html) into `docs/specs/NNN-slug/mockups/option-a.html`, `-b`, `-c`. One must be the restrained option.

3. **Paste the design system in.** Copy [src/Frontend/src/design-system.css](../../../src/Frontend/src/design-system.css) verbatim and in full into the shell's `DESIGN SYSTEM` block. The client builds from the same file, so the mockup looks exactly like the app will. Read it first, and read [the frontend-design skill](../frontend-design/SKILL.md) for the rules of the hand-drawn style.

4. **Build only from the design system**: the component classes and decorations its header lists, and Tailwind utilities on its tokens (`bg-postit`, `rounded-wobbly`, `shadow-hard`). No raw hex colours, no one-off styles. If an option genuinely needs something new, add it under `PROPOSED ADDITIONS` and name it in the comparison — picking that option means adding it to `design-system.css`.

5. **Show every state** in each file: populated, empty, loading, error, and the awkward edges — a very long Game name, a large number, a zero balance, a single item.

6. **Realistic content, glossary copy.** Real board game names, prices as `49,99 €`. UI text is bound by [docs/glossary.md](../../../docs/glossary.md). Comment what the markup cannot show.

7. **Open all three in the browser.** Reading the HTML is not looking at it.

   ```powershell
   Get-ChildItem docs/specs/NNN-slug/mockups/*.html | ForEach-Object { Start-Process $_.FullName }
   ```

8. **Stop and wait.** In chat: a comparison table — idea, best when, fails when, cost to build, proposed additions to the design system — and a recommendation with what would change your mind. **The human picks.** Do not decide for them, and do not start implementing.

9. **Once picked**, copy the winner to `docs/design/<component>.html`; it is the reference implementations are checked against. Losers never go there.

10. **No Angular. Do not edit `src/`** — reading `design-system.css` to paste it is the only contact with it. Proposed additions reach it through the implementation issue, not through the mockup.
