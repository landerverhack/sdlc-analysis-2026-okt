using GameStore.Domain.Customers;
using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;
using GameStore.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Infrastructure;

internal sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
  public DbSet<Game> Games => Set<Game>();
  public DbSet<Publisher> Publishers => Set<Publisher>();
  public DbSet<ShoppingBasket> ShoppingBaskets => Set<ShoppingBasket>();
  public DbSet<Order> Orders => Set<Order>();
  public DbSet<Customer> Customers => Set<Customer>();

  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    configurationBuilder.Properties<GameId>().HaveConversion<GameIdConverter>();
    configurationBuilder.Properties<PublisherId>().HaveConversion<PublisherIdConverter>();
    configurationBuilder.Properties<ShoppingBasketId>().HaveConversion<ShoppingBasketIdConverter>();
    configurationBuilder.Properties<BasketLineId>().HaveConversion<BasketLineIdConverter>();
    configurationBuilder.Properties<OrderId>().HaveConversion<OrderIdConverter>();
    configurationBuilder.Properties<OrderLineId>().HaveConversion<OrderLineIdConverter>();
    configurationBuilder.Properties<CustomerId>().HaveConversion<CustomerIdConverter>();
    configurationBuilder.Properties<EmailAddress>().HaveConversion<EmailAddressConverter>().HaveMaxLength(256);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
  }

}
