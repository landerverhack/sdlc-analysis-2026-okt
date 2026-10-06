# Wishlist

- **Spec:** 001-wishlist
- **Status:** draft
- **Date:** 2026-10-06

## Goal

Customers can keep Games for later consideration without committing to buy them.

## Scope

**In scope**

- One persistent Wishlist per existing Customer, selected by `customerId`.
- Saving or removing Games from the catalogue or Game details.
- A reachable Wishlist view with Game details, current prices, availability,
  removal, and adding one copy to the Shopping Basket.
- Newest saves first, without a business-defined size limit.
- Populated, empty, loading and error states using the existing design system.
- Behavioural acceptance criteria as documentation, not an automated test suite.

**Out of scope**

- Visitors, sign-in, authentication, ownership verification and Customer switching.
  The existing client keeps its fixed Customer; the feature accepts `customerId`.
- Multiple named Wishlists, sharing, alerts, notes, bulk actions and administrator editing.
- Desired quantities, stock reservation, price guarantees, currency conversion,
  combined totals and Wishlist-specific discounts.
- Automatic removal following purchase, cancellation or refund.
- A Game-deletion feature; only the Wishlist's response to a missing Game is covered.
- Changes to existing Shopping Basket entry points or Checkout pricing rules.
- Renaming existing HTTP fields or Shopping Basket methods.
- Test projects, test frameworks, test code and automated feature tests, per the
  feature owner's explicit decision. Passing acceptance criteria is not claimed.

## Requirements

1. **R1** - Each Customer has one Wishlist.
2. **R2** - A Customer's Wishlist persists across visits from different devices.
3. **R3** - Changing one Customer's Wishlist does not change another Customer's Wishlist.
4. **R4** - A Game appears at most once in a Wishlist.
5. **R5** - A Customer can save a Game to the Wishlist from the catalogue or Game details.
6. **R6** - A Customer can remove a Game from the Wishlist from the catalogue or Game details.
7. **R7** - Saving an already-saved Game leaves the Wishlist unchanged.
8. **R8** - Removing a Game absent from the Wishlist leaves the Wishlist unchanged.
9. **R9** - Games in a Wishlist appear most recently saved first.
10. **R10** - Saving a Game does not change its Stock Quantity.
11. **R11** - A Game with zero Stock Quantity can remain in a Wishlist.
12. **R12** - A Wishlist shows each Game's current Unit Price with its Currency.
13. **R13** - A Wishlist may contain Games with different Currencies.
14. **R14** - Adding a saved Game to the Shopping Basket adds one copy.
15. **R15** - Adding a saved Game to the Shopping Basket leaves the Wishlist unchanged.
16. **R16** - Adding a saved Game with zero Stock Quantity to the Shopping Basket is rejected.
17. **R17** - Checkout leaves the Wishlist unchanged.
18. **R18** - A permanently deleted Game is automatically removed from the Wishlist.
19. **R19** - A Customer can view the Wishlist.
20. **R20** - A Customer can open a saved Game's details from the Wishlist.
21. **R21** - A Customer can remove a Game from the Wishlist while viewing the Wishlist.
22. **R22** - A failed change to the Wishlist preserves the Wishlist.

## Acceptance criteria

**R1**

- Given an existing Customer with no saved Games, opening the Wishlist shows an
  empty Wishlist rather than requiring the Customer to create one.
- Given an existing Customer with saved Games, repeated visits show that same Wishlist.

**R2**

- Given a Game successfully saved for a Customer, a fresh visit using the same
  `customerId` on a different device shows the saved Game.
- Removing it successfully on one device means it is absent when the other device
  next loads the Wishlist; real-time synchronisation is not promised.

**R3**

- Given two existing Customers, saving or removing a Game for one `customerId`
  leaves the other Customer's saved Games unchanged.
- This checks separation by identifier, not access authorisation: someone supplying
  another Customer's `customerId` is not prevented from acting as that Customer.

**R4**

- Saving the same Game repeatedly never shows duplicate Games in the Wishlist.
- Two concurrent saves of the same Game still result in at most one saved Game.

**R5**

- Saving an unsaved Game from the catalogue makes it appear in the Wishlist.
- Saving an unsaved Game from its details makes it appear in the Wishlist.
- A Game with zero Stock Quantity can be saved from either location.

**R6**

- Removing a saved Game from the catalogue makes it absent from the Wishlist.
- Removing a saved Game from its details makes it absent from the Wishlist.

**R7**

- Saving an already-saved Game succeeds without duplicating it or moving it ahead
  of more recently saved Games.

**R8**

- Removing an existing Game that is not saved succeeds without changing any saved Games.
- Repeating a successful removal succeeds without changing any other saved Games.

**R9**

- Saving Game A followed by Game B shows B before A.
- Removing A then saving A again shows A before B.
- Games saved at the same UTC time appear in ascending Game identity order.

**R10**

- Saving a Game leaves its Stock Quantity unchanged, including when it is zero.

**R11**

- A saved Game that becomes out of stock remains visible with an out-of-stock state.
- If its Stock Quantity becomes positive, the next Wishlist load shows it as available.

**R12**

- Opening the Wishlist shows the current Unit Price with its Currency for each Game.
- Changing a Game's price or Currency after saving it is reflected on the next
  Wishlist load; the originally saved price is not shown as a price guarantee.
- No combined total or Wishlist Discount is displayed, including for a VIP Customer.

**R13**

- Saving an EUR Game followed by a USD Game succeeds; both remain visible with
  their respective Currencies.
- Adding them to the Shopping Basket does not bypass existing Checkout Pricing:
  mixed currencies are rejected at Pricing, not necessarily at basket addition.
  Current behaviour: [src/Backend/src/GameStore.Domain/Services/OrderPricingService.cs:22](../../../src/Backend/src/GameStore.Domain/Services/OrderPricingService.cs#L22).

**R14**

- Adding an available saved Game absent from the Shopping Basket creates a Basket
  Line with quantity one.
- Adding an available saved Game already present increases its Basket Line quantity
  by one. Current behaviour: [src/Backend/src/GameStore.Domain/Entities/ShoppingBasket.cs:47](../../../src/Backend/src/GameStore.Domain/Entities/ShoppingBasket.cs#L47).
- A failed addition reports an error without claiming that a copy was added.
  The client does not automatically retry an addition: a repeated successful
  addition would increase the quantity again.

**R15**

- After a successful Shopping Basket addition, the Game is still saved.
- After a rejected or failed Shopping Basket addition, the Game is still saved.

**R16**

- A saved Game with zero Stock Quantity has a disabled Shopping Basket action.
- If stock reaches zero after the view loaded but before the addition is handled,
  the addition is rejected without changing the Shopping Basket.
- This is a new rule for additions from the Wishlist, not an existing basket rule:
  [src/Backend/src/GameStore.Application/UseCases/AddItemToBasket.cs:37](../../../src/Backend/src/GameStore.Application/UseCases/AddItemToBasket.cs#L37)
  checks existence, then adds without checking stock. Other entry points remain unchanged.
- Positive stock does not reserve a copy or guarantee a later successful Checkout.

**R17**

- After successful Checkout, the Shopping Basket is empty while saved Games remain
  in the Wishlist. Existing basket clearing:
  [src/Backend/src/GameStore.Application/UseCases/Checkout.cs:54](../../../src/Backend/src/GameStore.Application/UseCases/Checkout.cs#L54).
- A failed Checkout leaves saved Games unchanged.
- Later Order cancellation does not restore or remove saved Games. Refund processing
  is outside this feature; it does not create a Wishlist action.

**R18**

- Following permanent Game deletion, the Game disappears without a Customer removal
  action; no unavailable historical entry remains.
- A Game absent from the persisted catalogue is removed no later than the next
  successful Wishlist load. An out-of-stock Game is not a deleted Game.

**R19**

- The Customer can open a populated Wishlist showing their saved Games.
- An empty Wishlist shows its empty state; a pending load shows its loading state.
- A failed load shows an error state rather than presenting failure as an empty Wishlist.
- Existing screens: [src/Frontend/src/app/app.routes.ts:3](../../../src/Frontend/src/app/app.routes.ts#L3).

**R20**

- Opening a saved Game from the Wishlist navigates to that Game's existing details.
- If the Game disappears before navigation completes, no stale successful details
  view is claimed; missing-Game handling remains visible.

**R21**

- Removing a Game in the Wishlist makes it disappear after a successful removal.
- Failure shows an error without presenting the Game as successfully removed.

**R22**

- A save rejected before it succeeds leaves previously saved Games unchanged.
- A removal rejected before it succeeds leaves previously saved Games unchanged.
- A failed action shows an error instead of success. If the response is lost after
  persistence, the client reloads the Wishlist to establish its current state
  rather than claiming rollback.
- Concurrent saving and removal of the same Game take effect in successful commit
  order; the last successful commit determines whether the Game is saved.

## Constraints

- Domain language follows [the glossary](../../glossary.md). Wishlist is accepted
  with a planned domain code home; no code or type sketch is part of this spec.
  Existing `Title` and `Item` names are not extended into new contracts; existing
  contracts remain compatible. The glossary's removed audit notes are not restored.
- Backend layering: [ADR-0001](../../adr/0001-clean-architecture-layering.md).
- Identifiers and Money: [ADR-0002](../../adr/0002-strongly-typed-ids-and-value-objects.md).
- Domain invariants: [ADR-0003](../../adr/0003-aggregates-own-their-invariants.md).
- Wishlist ownership: the approved planning choice is a separate Wishlist
  Aggregate; Customer and Shopping Basket remain independent. Per the feature
  owner's instruction on 2026-10-06, this choice is recorded in this spec without
  creating or requiring acceptance of a new ADR.
- If Domain Events become necessary: [ADR-0004](../../adr/0004-domain-events-dispatched-after-commit.md).
  This feature does not require alerts or introduce an event requirement.
- Application operations: [ADR-0005](../../adr/0005-use-cases-as-single-method-interfaces.md).
- HTTP contracts and errors: [ADR-0006](../../adr/0006-minimal-api-endpoints-grouped-by-feature.md).
- Frontend stack, shared state and design system: [ADR-0007](../../adr/0007-frontend-stack.md).
  Keep the existing fixed Customer; cross-device persistence does not introduce a
  Customer selector or multiple simultaneous Customers in the client.
- Security: rely on supplied `customerId` only, as explicitly requested. Do not
  add authentication or ownership enforcement here. Customer separation is not
  confidentiality; identifier spoofing remains an accepted risk in this release.
  Existing identity convention: [src/Frontend/src/app/core/api.config.ts:4](../../../src/Frontend/src/app/core/api.config.ts#L4).
- Persistence compatibility: the existing backend uses SQLite with
  `EnsureCreatedAsync`. Follow the existing [Infrastructure instructions](../../../.github/instructions/infrastructure.instructions.md):
  do not add migrations or a Wishlist-specific schema upgrade. Adding the Wishlist
  model makes an existing `gamestore.db` stale; it must be deleted before startup
  recreates the schema. State this requirement when changing the model. Existing
  database data is lost on recreation; preserving it is outside this feature.
  [src/Backend/src/GameStore.Infrastructure/DependencyInjection.cs:19](../../../src/Backend/src/GameStore.Infrastructure/DependencyInjection.cs#L19);
  [src/Backend/src/GameStore.Infrastructure/DependencyInjection.cs:42](../../../src/Backend/src/GameStore.Infrastructure/DependencyInjection.cs#L42).
- Pricing compatibility: preserve existing mixed-currency rejection and weekend
  VIP Pricing; no new time-zone, Currency-conversion or rounding rule belongs here.
  [src/Backend/src/GameStore.Domain/Services/OrderPricingService.cs:22](../../../src/Backend/src/GameStore.Domain/Services/OrderPricingService.cs#L22);
  [src/Backend/src/GameStore.Application/UseCases/Checkout.cs:38](../../../src/Backend/src/GameStore.Application/UseCases/Checkout.cs#L38).
- Verification: no test work is requested. Acceptance criteria describe the expected
  behaviour but are not evidence it works. A future testing strategy requires its
  own decision; this feature does not introduce one.
- No numeric performance gate is imposed for this release. No business size limit
  does not promise unbounded resource use; do not silently cap or truncate saved Games.
- Option A is the accepted [Wishlist mockup](../../design/wishlist.html), the
  visual reference for implementation.
  The approved layout is a compact responsive list with Game Image, Game name,
  current Unit Price and Currency, Stock Quantity, opening Game details, removal,
  and adding to the Shopping Basket. Provide a Wishlist link in the shared
  navigation. Save/remove controls on the catalogue and Game details use the same
  saved state. Keep the existing design system, keyboard access and all load states
  shown in the accepted mockup.
- Save ordering uses server UTC time, with ascending Game identity breaking ties.
  Repeated saves retain their original ordering; remove then save establishes a new
  ordering. Concurrent changes follow successful commit order. No real-time updates
  or client-clock trust are introduced.
- No accepted ADR needs superseding. A new ADR is not a prerequisite for this
  feature, per the feature owner's instruction above.

## Open questions

The feature owner delegated the remaining choices, then approved the decisions and
the nine-issue plan on 2026-10-06. No business or design question remains unanswered.
The decisions below preserve the original question identifiers for traceability.

| # | Resolution | Blocks | Owner |
| --- | --- | --- | --- |
| Q1 | Wishlist is a separate Aggregate; the approved choice is recorded in this spec without a new ADR, as instructed by the feature owner on 2026-10-06. | None; resolved. | Repository maintainer |
| Q2 | Successful commit order determines conflicting changes; UTC save time with Game identity tie-breaking determines display order. Reload after a lost response. | None; resolved. | Feature owner with backend architect |
| Q3 | A Game absent from persistence disappears by the next successful Wishlist load. No deletion endpoint is added. Existing endpoint: [src/Backend/src/GameStore.Presentation/Endpoints/GamesEndpoints.cs:9](../../../src/Backend/src/GameStore.Presentation/Endpoints/GamesEndpoints.cs#L9). | None; resolved. | Feature owner with backend architect |
| Q4 | Follow the existing Infrastructure instructions: no migrations or Wishlist-specific schema upgrade. Delete the stale database after the model changes and recreate it with EnsureCreatedAsync; existing database data is lost. The feature owner corrected the earlier data-preservation requirement on 2026-10-06. | None; resolved. | Repository maintainer |
| Q5 | Option A, the compact responsive list, is accepted as the [Wishlist visual reference](../../design/wishlist.html). | None; resolved. | Feature owner with frontend designer |
| Q6 | No numeric performance gate or silent saved-Game cap in this release. | None; resolved. | Feature owner |

Authentication and tests remain deliberately deferred. No issue is blocked by an
unanswered question; implementation dependencies still apply.