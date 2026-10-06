# Ubiquitous language

The vocabulary of the GameStore domain. This file is **binding**: code, specs, ADRs,
issues, commit messages and UI copy all use these terms and only these terms.

**Adding a term.** If you need a concept that is not here, propose the addition and get
it accepted *before* writing code that uses it. A term is only real once it appears in
this file with a code home.

**Changing a term.** Renaming a term means renaming it everywhere, in the same change.
A term that means one thing in the domain and another in the API is a defect, not a
style preference.

> **Scope note.** This glossary was written from the **domain layer**
> (`src/Backend/src/GameStore.Domain/`), which is the authority on meaning. The
> Application, Infrastructure and Presentation layers have **not** been audited against
> it. Expect drift, and expect to find it. Run `/glossary-audit` before starting a
> feature that touches those layers.

---

## Catalogue

| Term | Meaning | Code home |
| --- | --- | --- |
| **Game** | A board game offered for sale. The unit of everything a customer browses, adds and buys. Has a name, a price, a stock quantity, an image and exactly one publisher. | `Entities/Game.cs` |
| **Publisher** | The company that publishes a Game. A Game has exactly one Publisher; a Publisher has many Games. Publishers create Games (`Publisher.CreateGame`) — a Game cannot exist without one. | `Entities/Publisher.cs` |
| **Stock Quantity** | How many copies of a Game are on hand. Never negative. Reduced when an Order is placed, never at the moment of adding to a Shopping Basket. | `Game.StockQuantity` |
| **Image** | An optional picture for a Game. When absent, a shared default image is used. | `ValueObjects/GameImage.cs` |

## People

| Term | Meaning | Code home |
| --- | --- | --- |
| **Customer** | A person who shops. Has a name, an Email Address, an optional Address, at most one Shopping Basket, and a VIP flag. | `Entities/Customer.cs` |
| **VIP** | The *only* customer segmentation that exists today: a boolean on Customer. Its sole effect is a 10% discount on weekend orders. It is not a tier, a level or a membership. | `Customer.IsVip` |
| **Email Address** | A validated, lower-cased email. Has an explicit `Empty` value rather than being nullable. | `ValueObjects/EmailAddress.cs` |
| **Address** | Where a Customer lives: street and city. Changing it is `MoveToNewAddress`, not a setter. | `ValueObjects/Address.cs` |

## Buying

| Term | Meaning | Code home |
| --- | --- | --- |
| **Shopping Basket** | The Games a Customer intends to buy, before they buy them. Mutable, belongs to exactly one Customer, emptied on checkout. Prices in a Basket are live — they follow the Game's current price. | `Entities/ShoppingBasket.cs` |
| **Basket Line** | One Game and a quantity within a Shopping Basket. Adding a Game already present increases the quantity rather than adding a second line. | `Entities/BasketLine.cs` |
| **Checkout** | The act of turning a Shopping Basket into an Order. Prices, discount and total are fixed at this moment; the Basket is cleared. | `UseCases/Checkout.cs` |
| **Order** | A purchase that has been committed. Immutable except for its Status. Carries its own copy of names and prices so that later Game changes cannot rewrite history. | `Entities/Order.cs` |
| **Order Line** | One Game, a quantity, and the Unit Price *as it was at checkout*, within an Order. Stores the Game's name as text — an Order Line does not reach back to the Game. | `Entities/OrderLine.cs` |
| **Order Number** | The human-facing identifier of an Order, shaped `ORD-YYYYMMDD-XXXXXXXX`. Distinct from the Order's database identity. | `Order.OrderNumber` |
| **Order Status** | Where an Order is in its life: **Placed** to **Confirmed** to **Shipped**, or **Cancelled**. Transitions are one-way and enforced; a Shipped Order cannot be cancelled. | `ValueObjects/OrderStatus.cs` |

## Money

| Term | Meaning | Code home |
| --- | --- | --- |
| **Money** | An amount together with its Currency. Never a bare `decimal`. Adding or subtracting across currencies is not supported. | `ValueObjects/Money.cs` |
| **Currency** | One of EUR, USD, JPY. A Shopping Basket must be single-currency; mixed baskets are rejected at pricing time. | `ValueObjects/CurrencyName.cs` |
| **Unit Price** | What one copy of a Game costs. On a Game it is `Price`; captured onto an Order Line at checkout as `UnitPrice`. | `Game.Price`, `OrderLine.UnitPrice` |
| **Line Total** | Unit Price times quantity, for one line. Always derived, never stored. | `BasketLine.LineTotal`, `OrderLine.LineTotal` |
| **Subtotal** | The sum of all Line Totals, before any Discount. | `ShoppingBasket.Subtotal`, `Order.Subtotal` |
| **Discount** | The single amount subtracted from an Order's Subtotal. Today there is exactly one source: the weekend VIP rule. An Order holds **one** Discount amount, not a list of them, and `ApplyDiscount` replaces that amount rather than adding to it. | `Order.Discount` |
| **Final Total** | Subtotal minus Discount. What the Customer owes. | `Order.FinalTotal` |
| **Pricing** | Working out Subtotal, Discount and Final Total for a Basket and a Customer. Lives in a domain service because it depends on both, plus the calendar. | `Services/OrderPricingService.cs` |

## Building blocks

These are architectural terms rather than business terms, but they are used precisely
and inconsistent use of them causes real confusion.

| Term | Meaning | Code home |
| --- | --- | --- |
| **Entity** | Something with identity that persists over time. Identity is a strongly-typed id, never a bare `int`. | `Abstractions/IEntity.cs` |
| **Value Object** | Something defined entirely by its values, with no identity: Money, Address, Email Address, and every `*Id`. | `ValueObjects/` |
| **Aggregate** | An Entity that owns a cluster of objects and guarantees their consistency. Shopping Basket and Order are aggregates; Basket Line and Order Line are only reachable through them. | — |
| **Domain Event** | A statement that something business-meaningful has happened, in the past tense: `OrderPlaced`, `GamePriceChanged`. | `Domain/Events/` |
| **Domain Service** | Business logic that belongs to no single Entity because it spans several. | `Abstractions/IDomainService.cs` |
| **Use Case** | One thing an actor can do, exposed as a single-method interface in the Application layer. | `Abstractions/IUseCase.cs` |
| **Repository** | The way an aggregate is loaded and saved. Interface in Application, implementation in Infrastructure. | `Application/Repositories/` |
| **Unit of Work** | The transaction boundary. A use case commits once. | `Abstractions/IUnitOfWork.cs` |

---

## Words we do not use

Each of these shows up naturally in conversation and in generated code. Each one has an
agreed replacement. Reject the left column on sight.

| Do not write | Write instead | Why |
| --- | --- | --- |
| Product, Item, Article, Title | **Game** | This store sells one kind of thing, and it has a name |
| Cart, Bag | **Shopping Basket** | One name for one concept |
| Cart Item, Basket Item | **Basket Line** | Lines have quantities; items do not |
| User, Client, Buyer, Shopper | **Customer** | `User` will eventually mean an authenticated principal, which is a different thing |
| Purchase, Sale, Transaction | **Order** | An Order is the record; the other words are events or accounting terms |
| Total (unqualified) | **Subtotal**, **Line Total** or **Final Total** | Bare "Total" is ambiguous in every conversation it appears in |
| Amount (unqualified), a bare `decimal` price | **Money** | An amount without a currency is not a price |
| Inventory, Quantity On Hand | **Stock Quantity** | One name for one concept |
| Premium, Gold, Member, Loyalty | **VIP** | Today there is only the boolean. If richer segmentation is introduced it needs a glossary entry first |
| Order Id (when you mean the customer-facing one) | **Order Number** | The Order also has an `OrderId` and they are not the same |

---

## Under review

Terms whose meaning is contested, drifting, or used differently in different layers.
Nothing may be implemented against a term in this section until it is resolved and
moved into the catalogue above.

`/glossary-audit` writes its findings here.

_(empty)_
