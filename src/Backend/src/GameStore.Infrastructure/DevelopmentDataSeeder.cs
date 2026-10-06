using GameStore.Domain.Customers;
using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Infrastructure;

/// <summary>
/// Fills an empty development database with a small catalogue, so the Angular client has
/// something to show. Everything is created through the domain's own factory methods:
/// a Game only exists because a Publisher created it.
/// Called from Program.cs in the Development environment only, and does nothing if the
/// database already holds Games.
/// </summary>
public static class DevelopmentDataSeeder
{
  public static async Task SeedDevelopmentDataAsync(
    this IServiceProvider serviceProvider,
    CancellationToken cancellationToken = default)
  {
    using var scope = serviceProvider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (await dbContext.Games.AnyAsync(cancellationToken))
      return;

    // The client has no authentication and shops as Customer 1, so this Customer must exist.
    var customer = new Customer(new CustomerId(0), "Ada", "Janssens")
    {
      Email = new EmailAddress("ada.janssens@example.com"),
    };
    customer.MoveToNewAddress(new Address("Grote Markt 1", "Mechelen"));
    customer.SetVipStatus(true);
    await dbContext.Customers.AddAsync(customer, cancellationToken);

    var daysOfWonder = new Publisher(new PublisherId(0), "Days of Wonder");
    var zManGames = new Publisher(new PublisherId(0), "Z-Man Games");
    var rioGrandeGames = new Publisher(new PublisherId(0), "Rio Grande Games");
    var kosmos = new Publisher(new PublisherId(0), "Kosmos");

    await dbContext.Publishers.AddRangeAsync(
      [daysOfWonder, zManGames, rioGrandeGames, kosmos],
      cancellationToken);

    AddGame(dbContext, daysOfWonder, "Ticket to Ride", 49.95m, 12);
    AddGame(dbContext, daysOfWonder, "Small World", 44.90m, 6);
    AddGame(dbContext, daysOfWonder, "Memoir '44", 59.95m, 3);

    AddGame(dbContext, zManGames, "Pandemic", 42.00m, 9);
    AddGame(dbContext, zManGames, "Carcassonne", 32.50m, 15);
    AddGame(dbContext, zManGames, "Love Letter", 12.95m, 25);

    AddGame(dbContext, rioGrandeGames, "Dominion", 44.95m, 8);
    AddGame(dbContext, rioGrandeGames, "Race for the Galaxy", 29.95m, 7);
    AddGame(dbContext, rioGrandeGames, "Power Grid", 42.95m, 5);

    AddGame(dbContext, kosmos, "Catan", 44.95m, 14);
    AddGame(dbContext, kosmos, "Lost Cities", 17.95m, 20);
    AddGame(dbContext, kosmos, "Andor: The Last Hope", 39.95m, 4);

    await dbContext.SaveChangesAsync(cancellationToken);
  }

  private static void AddGame(
    ApplicationDbContext dbContext,
    Publisher publisher,
    string name,
    decimal priceInEuro,
    int stockQuantity)
  {
    var game = publisher.CreateGame(name, new GameId(0));
    game.SetPrice(new Money(priceInEuro, CurrencyName.EUR));
    game.SetStock(stockQuantity);

    dbContext.Games.Add(game);
  }
}
