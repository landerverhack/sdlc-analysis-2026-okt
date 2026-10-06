using GameStore.Application.Abstractions;
using GameStore.Application.EventHandlers;
using GameStore.Application.Repositories;
using GameStore.Domain.Events;
using GameStore.Infrastructure.Messaging;
using GameStore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
  {
    var connectionString = configuration.GetConnectionString("GameStore") ?? "Data Source=gamestore.db";

    services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

    services.AddScoped<ICustomerRepository, CustomerRepository>();
    services.AddScoped<IGameRepository, GameRepository>();
    services.AddScoped<IOrderRepository, OrderRepository>();
    services.AddScoped<IPublisherRepository, PublisherRepository>();
    services.AddScoped<IShoppingBasketRepository, ShoppingBasketRepository>();

    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
    services.AddScoped<IEmailService, MockEmailService>();

    services.AddScoped<IDomainEventHandler<GamePriceChanged>, GamePriceChangedBasketRecalculationHandler>();
    services.AddScoped<IDomainEventHandler<OrderPlaced>, OrderPlacedEmailHandler>();
    services.AddScoped<IDomainEventHandler<OrderPlaced>, OrderPlacedStockReductionHandler>();

    return services;
  }

  public static async Task EnsureDatabaseCreatedAsync(this IServiceProvider serviceProvider)
  {
    using var scope = serviceProvider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
  }
}
