# Design

Accepted UI mockups, kept as documentation.

## How these files are used

A mockup here is not a sketch that was thrown away once the code was written. It is
**the specification of the visual result**, and it stays. When a component is built or
changed, the mockup is the reference:

```
Implement the product listing to match docs/design/listing-cards.html.
Use the design system classes it uses. Include the empty and loading states shown
in the mockup. Do not change the layout.
```

That instruction only works if the file is still here and still accurate. If the
implementation deliberately diverges, update the mockup in the same change — otherwise
the next person is told to match something that is no longer true.

## What belongs here

- **Only the accepted option.** Rejected alternatives are interesting during the
  decision and noise afterwards. If the comparison itself is worth keeping, put all the
  options under the spec that produced them (`docs/specs/NNN-slug/mockups/`) and copy
  only the winner here.
- **One self-contained HTML file per component or screen**, named for the thing it
  shows: `listing-cards.html`, `checkout-redemption.html`.
- **Every state the component can be in** — populated, empty, loading, error, and any
  interesting edge (long names, large numbers, zero balance). A mockup that shows only
  the happy path will produce an implementation that only handles the happy path.

## Conventions

- Tailwind's browser build from a CDN, with [src/Frontend/src/design-system.css](../../src/Frontend/src/design-system.css)
  pasted in, so the file opens in a browser with no build step and looks exactly like the
  client. The pasted copy is a snapshot: if the two disagree about how a component looks,
  the file in `src/` wins; the mockup still decides the layout.
- Realistic content. Real game names, real prices in the right currency format, the
  kind of names that are actually too long.
- Static HTML. No framework, no data fetching, no build tooling. The point is the
  visual result, not a working prototype.
- Annotate with HTML comments where behaviour is not visible from the markup — what a
  control does, what changes on hover, what the loading state is waiting for.

Generate a set of options to compare with `/mockups`.
