namespace GameStore.Presentation.Endpoints;

internal static class EndpointRegistration
{
  public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
  {
    app.MapCustomersEndpoints();
    app.MapPublishersEndpoints();
    app.MapGamesEndpoints();
    app.MapBasketsEndpoints();

    return app;
  }
}
