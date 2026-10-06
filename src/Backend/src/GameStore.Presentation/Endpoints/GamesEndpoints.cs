using GameStore.Application.UseCases;
using GameStore.Domain.Abstractions;
using GameStore.Domain.ValueObjects;

namespace GameStore.Presentation.Endpoints;

internal static class GamesEndpoints
{
  public static IEndpointRouteBuilder MapGamesEndpoints(this IEndpointRouteBuilder app)
  {
    var group = app.MapGroup("/api/games").WithTags("Games");

    group.MapPost("", async (CreateGameRequest request, ICreateGameUseCase useCase, CancellationToken cancellationToken) =>
    {
      try
      {
        if (!Enum.TryParse<CurrencyName>(request.Currency, true, out var currency))
          return Results.BadRequest(new { error = "Invalid currency" });

        var game = await useCase.CreateGame(
          request.Title,
          request.Price,
          currency,
          request.StockQuantity ?? 0,
          new PublisherId(request.PublisherId),
          imageUrl: null,
          cancellationToken);

        return Results.Created($"/api/games/{game.Id.Value}", new { id = game.Id.Value });
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

    group.MapPut("/{id:int}", async (int id, UpdateGameRequest request, IUpdateGameUseCase useCase, CancellationToken cancellationToken) =>
    {
      try
      {
        if (!Enum.TryParse<CurrencyName>(request.Currency, true, out var currency))
          return Results.BadRequest(new { error = "Invalid currency" });

        await useCase.UpdateGame(
          new GameId(id),
          request.Title,
          request.Price,
          currency,
          new PublisherId(request.PublisherId),
          imageUrl: null,
          cancellationToken);

        return Results.NoContent();
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

    group.MapGet("", async (IGetAllGamesUseCase useCase, CancellationToken cancellationToken) =>
    {
      var games = await useCase.GetAll(cancellationToken);

      return Results.Ok(games.Select(game => new
      {
        id = game.Id.Value,
        name = game.Name,
        price = game.Price.Amount,
        currency = game.Price.Currency.ToString(),
        stockQuantity = game.StockQuantity,
        publisherId = game.Publisher.Id.Value,
        publisherName = game.PublisherName,
        imageUrl = game.ImageURL,
      }));
    });

    group.MapGet("/{id:int}", async (int id, IGetGameByIdUseCase useCase, CancellationToken cancellationToken) =>
    {
      try
      {
        var game = await useCase.GetById(new GameId(id), cancellationToken);

        return Results.Ok(new
        {
          id = game.Id.Value,
          title = game.Name,
          price = game.Price.Amount,
          currency = game.Price.Currency.ToString(),
          stockQuantity = game.StockQuantity,
          publisherId = game.Publisher.Id.Value,
          publisherName = game.PublisherName,
          imageUrl = game.ImageURL,
        });
      }
      catch (DomainException)
      {
        return Results.NotFound();
      }
    });

    return app;
  }

  internal sealed record CreateGameRequest(
    string Title,
    string? Description,
    decimal Price,
    string Currency,
    int? StockQuantity,
    int PublisherId);

  internal sealed record UpdateGameRequest(
    string Title,
    string? Description,
    decimal Price,
    string Currency,
    int PublisherId);
}
