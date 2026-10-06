---
name: frontend-design
description: "Use when designing or styling a GameStore screen, component or mockup. Describes the hand-drawn style the client and every mockup are drawn in, and the rules that keep it consistent."
---

The GameStore looks **hand-drawn**: sketches on paper, sticky notes on a wall, a napkin
diagram. It should feel approachable and human, never corporate or clinical.

**The style is implemented in [src/Frontend/src/design-system.css](../../../src/Frontend/src/design-system.css)** —
tokens, decorations and component classes. Read it before designing anything; this file
only explains what the style is for, so you can extend it in the same spirit.

## Principles

- **No straight lines.** Containers, buttons and inputs use the wobbly radii
  (`rounded-wobbly`, `rounded-wobbly-md`, `rounded-blob`), never plain `rounded-*`.
- **Hard offset shadows, never blur** (`shadow-hard-sm`, `shadow-hard`, `shadow-hard-lg`).
  Hover reduces the offset; pressing removes it, so a button "presses flat".
- **Handwritten type only.** Kalam (`font-display`) for headings, Patrick Hand for body.
  Headings vary dramatically in size, like emphasised notes.
- **A limited palette:** pencil, paper, erased pencil, correction-marker red, ballpoint
  blue, post-it yellow. One light theme, no dark mode — that is part of the style.
- **Thick lines.** `border-2` minimum; `border-dashed` for secondary elements and dividers.
- **Slight tilt.** Small rotations (`rotate-1`, `-rotate-2`) on cards and decorations,
  and a quick jiggle on hover, break the rigid grid. Keep them small, and smaller on
  mobile.
- **Paper decorations:** `tape`, `tack` and the `scribble` underline, used sparingly to
  mark what matters.

## Rules

- Build from the design system's classes and tokens. No raw hex values, no inline
  `style`, no one-off CSS.
- If a screen genuinely needs something the design system lacks, propose it as a new
  component or token in `design-system.css` — on purpose, named, reusable.
- Keep it accessible: visible focus (the dashed ballpoint outline), touch targets of at
  least 48px, and respect `prefers-reduced-motion`.
- Mobile first: grids collapse to one column; purely decorative elements may hide below
  `md:`, but the wobbly borders, hard shadows and handwritten fonts never do.
