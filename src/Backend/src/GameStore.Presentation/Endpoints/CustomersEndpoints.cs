using GameStore.Application.UseCases;
using GameStore.Domain.Abstractions;
using GameStore.Domain.ValueObjects;

namespace GameStore.Presentation.Endpoints;

internal static class CustomersEndpoints
{
  public static IEndpointRouteBuilder MapCustomersEndpoints(this IEndpointRouteBuilder app)
  {
    var group = app.MapGroup("/api/customers").WithTags("Customers");

    group.MapPost("", async (CreateCustomerRequest request, ICreateCustomerUseCase useCase, CancellationToken cancellationToken) =>
    {
      try
      {
        var customer = await useCase.CreateCustomer(
          request.Email,
          request.FirstName,
          request.LastName,
          new Address(request.Address.Street, request.Address.City),
          request.IsVip ?? false,
          cancellationToken);

        return Results.Created($"/api/customers/{customer.Id.Value}", new { id = customer.Id.Value });
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

    return app;
  }

  internal sealed record CreateCustomerRequest(
    string Email,
    string FirstName,
    string LastName,
    bool? IsVip,
    string? PhoneNumber,
    AddressRequest Address);

  internal sealed record AddressRequest(
    string Street,
    string City,
    string? State,
    string? PostalCode,
    string? Country);
}
