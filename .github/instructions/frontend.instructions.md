---
name: Frontend
description: Rules for the Angular client, including implementing against an accepted mockup.
applyTo: "src/Frontend/**"
---

# Frontend

Angular client for the GameStore API. It lives in `src/Frontend/` and has four screens:
the Game catalogue, a Game's detail, the Shopping Basket and Checkout.

Binding decision: [ADR-0007](../../docs/adr/0007-frontend-stack.md).

## Language

UI copy is bound by [docs/glossary.md](../../docs/glossary.md) exactly as code is. A
button that says "Add to cart" is a defect: the glossary says Shopping Basket. This is
where domain language most often escapes, because copy feels like a writing decision
rather than a modelling one.

Component, service and model names use glossary terms too: `BasketLine`, not
`CartItem`.

## Design system

The client looks hand-drawn, and all of that look is defined in one file:
[src/Frontend/src/design-system.css](../../src/Frontend/src/design-system.css).
`styles.css` imports it after Tailwind, and every mockup pastes in the same file, so a
template and a mockup that use the same classes look the same.
[The frontend-design skill](../skills/frontend-design/SKILL.md) describes the style and
its rules.

- Build templates from the design system's **component classes** (`btn`, `card`,
  `alert`, `badge`, …) and **decorations** (`tape`, `tack`, `scribble`) — the file's
  header lists them — with Tailwind utilities for layout.
- Colours, fonts, radii and shadows come from its **tokens**, used through Tailwind:
  `bg-postit`, `text-marker`, `border-pencil`, `font-display`, `rounded-wobbly`,
  `shadow-hard`. No raw hex values, no arbitrary colours, no blurred shadows, no standard
  `rounded-*` corners on containers.
- No component stylesheets and no inline `style`. If something cannot be built from
  the design system, add it to `design-system.css` — a new component or token is a
  change to the design system, made on purpose, not a one-off in a template.
- No UI component library; [ADR-0007](../../docs/adr/0007-frontend-stack.md) says why.
- There is one light palette and no dark mode. That is part of the style, not a gap.

## Implementing against a mockup

Accepted mockups live in [docs/design/](../../docs/design/) and are the **reference**,
not a suggestion. When implementing a component that has one:

- Match the layout. Do not improve it in passing — if it is wrong, say so and change the
  mockup first.
- Use the same design-system classes and tokens as the mockup.
- A mockup carries a snapshot of `design-system.css` from the day it was made. Where
  the two disagree about how a component looks, the file in `src/` wins; the mockup
  still decides the layout. Anything under the mockup's `PROPOSED ADDITIONS` goes into
  `design-system.css` as part of the implementation.
- Implement **every state the mockup shows**: populated, empty, loading, error, and the
  edges. A mockup shows those states specifically so they do not get skipped.
- If the implementation has to diverge, update the mockup in the same change. Otherwise
  the next person is told to match something that is no longer true.

## API

The backend is at `http://localhost:5038`. It allows any origin in Development
(`Program.cs`), so there is no proxy here.

Responses are anonymous objects assembled by hand in the endpoints, so the wire shape is
whatever those files say. Read the endpoint before writing the client model; do not
assume it mirrors the domain. Ids arrive unwrapped (`id: 3`), and money arrives split
into `price` and `currency`.

There is no authentication. `customerId` is passed in the route.

## Stack

[ADR-0007](../../docs/adr/0007-frontend-stack.md) has the reasoning:

- Components inject a service, never `HttpClient`.
- Shared state lives in `core/*.service.ts` as read-only signals with `computed`
  derivations. A component keeps only state that is its own.
- Each screen carries one `LoadState` (`idle | loading | loaded | error`). Empty is not
  one of them: a screen is `loaded` and asks its data whether it is empty.
- Wire shapes stay private to the service that calls the endpoint and are mapped onto
  glossary-named models on the way in.
