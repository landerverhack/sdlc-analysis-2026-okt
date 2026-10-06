# GameStore client

The Angular client for the GameStore API. Four screens: the Game catalogue, a Game's
detail, the Shopping Basket, and Checkout.

The stack and the reasoning behind it are recorded in
[ADR-0007](../../docs/adr/0007-frontend-stack.md). The rules that apply while editing
these files are in
[.github/instructions/frontend.instructions.md](../../.github/instructions/frontend.instructions.md).

## Running it

```bash
npm install
npm start     # http://localhost:4200
npm run build # production build into dist/
```

The API has to be running on `http://localhost:5038`:

```bash
dotnet run --project ../Backend/src/GameStore.Presentation
```

It allows any origin in Development, so there is no proxy configuration here.

## Layout

```
src/app/
  app.ts, app.html          the shell: header, Shopping Basket count, router outlet
  app.routes.ts             four lazily loaded routes
  core/
    api.config.ts           API base URL and the hardcoded CUSTOMER_ID
    load-state.ts           idle | loading | loaded | error
    http-error.ts           turns a failed request into a sentence
    models/                 Game, ShoppingBasket, BasketLine
    game.service.ts         the Game catalogue, in signals
    shopping-basket.service.ts   the one Shopping Basket, in signals
  shared/money.pipe.ts      formats an amount together with its currency
  features/
    game-catalogue/  game-detail/  shopping-basket/  checkout/
```

## Things worth knowing before changing anything

- **The glossary is binding for UI copy.** A button that says "Add to cart" is a defect:
  the term is Shopping Basket. See [docs/glossary.md](../../docs/glossary.md), and in
  particular its "Words we do not use" table.
- **Every screen handles loading, empty and error.** That is what `LoadState` is for.
  Empty is not one of its values — a screen is `loaded` and asks its own data whether
  there is anything in it.
- **Only the services call the API.** Components inject a service, never `HttpClient`.
- **The wire shapes are not the models.** `GET /api/games/{id}` and the Basket Lines in
  `GET /api/baskets/{customerId}` call a Game's name `title`. It is renamed to `name` in
  the service that reads it, so the rest of the client never sees the word.
- **What the API cannot do yet**, and therefore neither can this client: remove a Basket
  Line or lower its quantity, and report the Order Number, Discount or Final Total after
  Checkout — that endpoint answers `202` with no body.
