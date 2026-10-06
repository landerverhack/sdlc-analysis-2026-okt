# GameStore Demo (Clean Architecture)
This is a small demo showing Domain + Application + Infrastructure + Presentation separation.

## Projects
- `GameStore.Domain`: business model and rules (`Game`, `Order`, `ShoppingBasket`, `Customer`).
- `GameStore.Application`: use-case orchestration (`CheckoutUseCase`, `CreateGameUseCase`) and contracts.
- `GameStore.Infrastructure`: EF Core, repositories, event publishing, email mock.
- `GameStore.Presentation`: minimal API endpoints grouped by feature.

## How requests flow
1. Endpoint receives HTTP request (for example `CustomersEndpoints`, `SalesBasketsEndpoints`).
2. Endpoint calls a use case interface (for example `ICheckoutUseCase`).
3. Use case loads aggregates via repository interfaces (`IShoppingBasketRepository`, `IOrderRepository`).
4. Domain entities enforce invariants (`ShoppingBasket`, `Order`, `Game`).
5. `IUnitOfWork` commits and `IDomainEventPublisher` dispatches events.

## Domain events (demo)
- `OrderPlacedEvent` triggers handlers like stock reduction and confirmation email.
- Handlers implement `IDomainEventHandler<TEvent>`.
- `MockEmailService` simulates email delivery.

## Domain service example
- `IOrderPricingService : IDomainService` in Domain.
- `OrderPricingService` applies a weekend VIP discount rule during checkout.

## Notes
- This demo favors clarity over completeness; conventions are intentionally simple.
- Start in `src/GameStore.Presentation/Program.cs` and `GameStore.Presentation.http` for end-to-end scenarios.
