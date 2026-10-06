# ADR-0007: The client is an Angular application with standalone components, Tailwind with a hand-drawn design system of our own, and state in signals

- **Status:** accepted
- **Date:** 2026-09-24
- **Applies to:** frontend

## Context

`src/Frontend/` has four screens in it: the Game catalogue, a
Game's detail, the Shopping Basket, and Checkout. They talk to the API at
`http://localhost:5038`, which has no authentication, so the client shops as a single
hardcoded Customer.

Four things are already fixed and the client has to live with them:

- The client has a look: the hand-drawn style described in
  [the frontend-design skill](../../.github/skills/frontend-design/SKILL.md) — paper
  and pencil colours, handwritten fonts, wobbly borders that are never straight, hard
  offset shadows with no blur, tape and thumbtacks. It is deliberately not the look of
  any component library.
- Mockups are static HTML files that open in a browser with no build step
  ([docs/design/](../design/)), and
  [the frontend instructions](../../.github/instructions/frontend.instructions.md) say
  an implementation matches the mockup it implements. "Match the mockup" is only a
  checkable instruction if a mockup and a template that use the same class names look
  the same.
- Every mockup has to show the populated, empty, loading and error states, and the
  implementation has to have all of them. So the client needs a way of saying "which
  state is this screen in" that does not turn into four unrelated booleans per
  component.
- The API is small and hand-assembled ([ADR-0006](0006-minimal-api-endpoints-grouped-by-feature.md)).
  There is one list endpoint, one detail endpoint, one add-to-basket call and one
  checkout call. The wire shapes do not mirror the domain: ids arrive unwrapped, Money
  arrives as a `price`/`currency` pair, and the Game's name arrives as `title` on two of
  the four responses.

The frontend instructions already name Angular and Tailwind, and say the remaining
choices — state management, HTTP layer, routing, where the look comes from — are open,
and that each one is an ADR rather than a decision made quietly inside a component.

The audience is a training course. What is here is the starting point students are
handed before they do analysis and design exercises on top of it. It has to be readable
after five minutes of looking at it, and it has to be something a room full of .NET
developers can follow.

## Considered options

The framework itself was not genuinely open — the instruction file says Angular and the
repository map says Angular. What is open is *how* the Angular application is put
together, and where its look comes from. Options A–C are about the first question,
D and E about the second.

### Option A: Standalone components, signals, and HttpClient in services

No `NgModule`. Routes are an array of lazy `loadComponent` entries. Two injectable
services hold what the whole client shares — the Game catalogue and the one Shopping
Basket — as signals, exposed read-only. Screens read those signals directly in their
templates and keep their own local signals for anything that is only theirs, such as
which Add button is currently spinning.

Costs: state lives in a service by convention and nothing enforces it, so an
undisciplined component can start fetching for itself and the client ends up with two
truths about the Shopping Basket. There are no devtools showing a state history. And
because each screen owns its own load state, the loading and error markup is written out
four times instead of shared.

What breaks it: a second Customer, offline support, or optimistic updates that need
rollback. Signals in a service have no answer to any of those.

### Option B: NgRx

A store, actions, reducers, effects and selectors. Time-travel debugging, one place
where state changes are visible, and a pattern that scales well past this app.

Costs: four to six files per feature, for a client with four screens and three write
calls, plus a package and its idioms. On a course whose subject is analysis and design,
the first thing a student would have to learn is NgRx, which is not the subject. It buys
insurance against complexity this client does not have.

What breaks it: nothing technical. It is rejected on proportion, not on capability.

### Option C: RxJS subjects in services and the async pipe

A `BehaviorSubject` per piece of state, `Observable` out, `| async` in the template. The
idiom most Angular code in the wild is written in, and no extra package.

Costs: anything derived needs operators — a Shopping Basket count derived from lines
derived from a request is `combineLatest` and `map` where signals are one `computed`.
Templates fill with `| async` and `*ngIf="x$ | async as x"`. For this size of
application it is the harder version of Option A, with the same guarantees.

### Option D: Angular Material

A component library with accessibility and keyboard behaviour built in, which is a real
advantage over hand-assembled markup.

Rejected: Material has a strong look of its own, and theming it into irregular borders
and handwritten fonts means overriding most of what it draws. It also needs a build, so
a mockup could not use it and "match the mockup" would stop being checkable.

### Option E: A design system of our own, in one Tailwind file

One stylesheet, written in Tailwind syntax, that is the whole look: theme tokens
(fonts, colours, wobbly radii, hard shadows), three decorations (`tape`, `tack`,
`scribble`), and the handful of component classes the screens need (`btn`, `card`,
`alert`, `badge`, `table`, `link`, `skeleton`, `spinner`).

Costs: we own every component, including the parts a library would have given us —
focus styles, reduced-motion handling, disabled states. There are no interactive
components (dialogs, menus, dropdowns); the first one that is needed is built by hand
or becomes a new decision. And the mockups cannot import the file — the Tailwind
browser build only compiles CSS written inline in the page — so each mockup carries a
copy.

## Decision

Option A for how the client is put together, and Option E for where its look comes
from, with Tailwind 4 compiled through PostCSS.

- **Standalone components**, no `NgModule`. Each of the four screens is one component
  with a separate `.html` template, lazily loaded by the router.
- **State in signals, in two services.** `GameService` holds the Game catalogue.
  `ShoppingBasketService` holds the one Shopping Basket and is the only thing that calls
  the basket endpoints. Both expose `asReadonly()` signals plus `computed` derivations,
  so a component can read state but can only change it by calling a method.
- **One explicit `LoadState`** — `idle | loading | loaded | error` — per screen, instead
  of separate loading and error booleans, because the states the mockups demand are the
  values of that type. Empty is deliberately not one of them: it is `loaded` with
  nothing in it, and each screen asks its own data whether it is empty.
- **`HttpClient` with `withFetch()`**, called from the services only. No component
  injects it.
- **The wire shapes stay in the service that calls the endpoint**, as private
  `...Response` interfaces, mapped onto glossary-named models on the way in. The `title`
  field the API uses for a Game's name is renamed to `name` at that boundary, so a word
  the glossary rejects never reaches a component or a template.
- **The look lives in `src/design-system.css` and nowhere else.** It is written in
  Tailwind syntax with no import of its own. `src/styles.css` is `@import "tailwindcss"`
  followed by an import of that file. Component classes sit in Tailwind's `components`
  layer, so a utility written next to one in a template always wins. Templates contain
  no raw colours, no component stylesheets and no inline styles.
- **Mockups use the same file.** The mockup shell loads Tailwind's browser build (pinned
  to the same minor version as the client) and has the contents of `design-system.css`
  pasted into a `<style type="text/tailwindcss">` block. A class in a mockup and the same
  class in a template are compiled from the same source by the same version of Tailwind.
- **No UI component library**, and one light palette with no dark mode, because the
  style has one.
- **`customerId` is a constant** in `core/api.config.ts`, next to the API base URL, so
  that when authentication arrives there is exactly one place that has to change.

What separated Option A from Option B was proportion. This client has three write
operations and one shared aggregate. NgRx is the right answer to a state problem, and
this client does not have one yet. What separated Option E from Option D was the same
thing from the other side: a library is worth it when you use its look, and this client
does not.

## Consequences

- A screen is one `.ts` file and one `.html` file, and the whole look is one CSS file of
  a few hundred lines. Reading the whole client is an afternoon at most, which is what a
  starting point for an exercise has to be.
- Derived values — the Shopping Basket count in the header, whether the Basket is empty
  — are one `computed` each and update everywhere at once, with no subscription
  management and no `ngOnDestroy`.
- The loading and error markup is copied into all four templates. That is real
  duplication and it gets worse with the fifth screen. Lifting it into shared components
  is the obvious next move and is deliberately not done yet.
- Nothing prevents a future component from injecting `HttpClient` directly and keeping
  its own copy of the Shopping Basket. The rule that the service owns shared state is a
  convention held up by review, not by the compiler.
- Changing how a button looks is one edit in one file, and it changes every screen. So
  does breaking it. A new component or token is a change to the design system, not
  something done in a template.
- Accessibility is ours to get right. Focus rings, disabled states and reduced motion
  are defined in `design-system.css`; nothing supplies them for free, and a new
  interactive component has to bring its own keyboard behaviour.
- There is no test setup. `ng new` left Vitest in `package.json` and there are no specs.
  How the frontend is tested is an open decision and needs its own ADR before anyone
  writes the first one.
- Each mockup holds a snapshot of `design-system.css` from the day it was made. When the
  file changes, accepted mockups in `docs/design/` go on showing the old version until
  someone refreshes them. The rule for a disagreement is that the file decides how a
  component looks and the mockup decides the layout; nothing checks that automatically.
- The fonts are loaded from Google Fonts, in `index.html` and in every mockup. Offline,
  both fall back to a generic cursive font.
- **What we would have to undo if this turns out to be wrong:** the two services, and
  the component classes. Every component reads the services through signals, so moving
  to a store means rewriting both services and every template binding that reads them.
  Moving to a component library means rewriting every template's markup, since the class
  names are ours. Each is a day at four screens and considerably more later.
- **What would tell us it has turned out to be wrong:** two components disagreeing about
  the Shopping Basket; a `computed` chain nobody can follow; needing to undo an
  optimistic update; or more than one Customer being signed in at a time. Any of those
  means the state problem has arrived and Option B was the answer to it. On the styling
  side: needing dialogs, menus or other interactive components with real keyboard
  behaviour, or `design-system.css` growing past what one person can read in one sitting.

## Revisit when

Authentication lands and `CUSTOMER_ID` stops being a constant; the client grows past
roughly eight screens; or the first interactive component is needed that a hand-written
class cannot do accessibly — whichever comes first.
