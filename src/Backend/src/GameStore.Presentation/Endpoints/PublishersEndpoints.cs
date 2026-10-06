using GameStore.Application.UseCases;
using GameStore.Domain.Abstractions;

namespace GameStore.Presentation.Endpoints;

internal static class PublishersEndpoints
{
  public static IEndpointRouteBuilder MapPublishersEndpoints(this IEndpointRouteBuilder app)
  {
    var group = app.MapGroup("/api/publishers").WithTags("Publishers");

    group.MapPost("", async (CreatePublisherRequest request, ICreatePublisherUseCase useCase, CancellationToken cancellationToken) =>
    {
      try
      {
        var publisher = await useCase.CreatePublisher(request.Name, cancellationToken);
        return Results.Created($"/api/publishers/{publisher.Id.Value}", new { id = publisher.Id.Value });
      }
      catch (DomainException ex)
      {
        return Results.BadRequest(new { error = ex.Message });
      }
    });

    return app;
  }

  internal sealed record CreatePublisherRequest(string Name, string? Country, string? Website);
}
