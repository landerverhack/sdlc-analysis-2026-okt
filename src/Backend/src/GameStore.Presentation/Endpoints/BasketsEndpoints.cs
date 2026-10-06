using GameStore.Application.UseCases;
using GameStore.Domain.Abstractions;
using GameStore.Domain.ValueObjects;

namespace GameStore.Presentation.Endpoints;

internal static class BasketsEndpoints
{
  public static IEndpointRouteBuilder MapBasketsEndpoints(this IEndpointRouteBuilder app)
  {
    var group = app.MapGroup("/api/baskets").WithTags("Baskets");

    group.MapPost("/{customerId:int}/items", async (int customerId, AddBasketItemRequest request, IAddItemToBasketUseCase useCase, CancellationToken cancellationToken) =>
    {
      try
      {
        var basket = await useCase.AddItem(
          new CustomerId(customerId),
          new GameId(request.GameId),
          request.Quantity,
          cancellationToken);

        return Results.Ok(new { id = basket.Id.Value, customerId = basket.CustomerId.Value });
      }
      catch (DomainException ex)
      {
        return Results.BadRequest(new { error = ex.Message });
      }
      catch (ArgumentException ex)
      {
        return Results.BadRequest(new { error = ex.Message });
      }
    });

    group.MapGet("/{customerId:int}", async (int customerId, IGetBasketByCustomerUseCase useCase, CancellationToken cancellationToken) =>
    {
      try
      {
        var basket = await useCase.GetByCustomerId(new CustomerId(customerId), cancellationToken);

        var lines = basket.Lines
          .Select(l => new
          {
            gameId = l.GameId.Value,
            title = l.Game.Name,
            quantity = l.Quantity,
            unitPrice = l.Game.Price.Amount,
            lineTotal = l.LineTotal.Amount,
            currency = l.Game.Price.Currency.ToString(),
          })
          .ToList();

        return Results.Ok(new
        {
          id = basket.Id.Value,
          customerId = basket.CustomerId.Value,
          subtotal = basket.Subtotal.Amount,
          currency = basket.Subtotal.Currency.ToString(),
          lines,
        });
      }
      catch (DomainException)
      {
        return Results.NotFound();
      }
    });

    group.MapPost("/{customerId:int}/checkout", async (int customerId, ICheckoutUseCase useCase, CancellationToken cancellationToken) =>
    {
      try
      {
        await useCase.CheckoutBasket(new CustomerId(customerId), cancellationToken);
        return Results.Accepted();
      }
      catch (DomainException ex)
      {
        return Results.BadRequest(new { error = ex.Message });
      }
    });

    return app;
  }

  internal sealed record AddBasketItemRequest(int GameId, int Quantity);
}
