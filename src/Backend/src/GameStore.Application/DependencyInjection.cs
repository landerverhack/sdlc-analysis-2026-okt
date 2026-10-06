using GameStore.Application.UseCases;
using GameStore.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Application;

public static class DependencyInjection
{
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    services.AddScoped<ICheckoutUseCase, CheckoutUseCase>();
    services.AddScoped<ICreateCustomerUseCase, CreateCustomerUseCase>();
    services.AddScoped<ICreatePublisherUseCase, CreatePublisherUseCase>();
    services.AddScoped<ICreateGameUseCase, CreateGameUseCase>();
    services.AddScoped<IUpdateGameUseCase, UpdateGameUseCase>();
    services.AddScoped<IAddItemToBasketUseCase, AddItemToBasketUseCase>();
    services.AddScoped<IGetGameByIdUseCase, GetGameByIdUseCase>();
    services.AddScoped<IGetAllGamesUseCase, GetAllGamesUseCase>();
    services.AddScoped<IGetBasketByCustomerUseCase, GetBasketByCustomerUseCase>();

    services.AddScoped<IOrderPricingService, OrderPricingService>();

    return services;
  }
}
