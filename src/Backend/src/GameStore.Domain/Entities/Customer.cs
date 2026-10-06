using GameStore.Domain.Customers;
using GameStore.Domain.ValueObjects;

namespace GameStore.Domain.Entities;

public sealed class Customer : IEntity<CustomerId>
{
  public Customer(CustomerId id, string firstName, string lastName)
  {
    Id = id;
    FirstName = firstName;
    LastName = lastName;
  }

  public CustomerId Id { get; private set; }
  public string FirstName { get; private set; }
  public string LastName { get; private set; }
  public bool IsVip { get; private set; }
  public EmailAddress Email { get; set; } = EmailAddress.Empty;
  public Address? Address { get; private set; }
  public ShoppingBasket? ShoppingBasket { get; set; }

  public void SetVipStatus(bool isVip)
  {
    IsVip = isVip;
  }

  public void MoveToNewAddress(Address address)
  {
    // Here we can do additional logic like validating the address,
    // checking if it's different from the current one, storing address history, etc.
    Address = address;
  }

  public void AddGameToShoppingBasket(Game game)
  {
    ShoppingBasket ??= ShoppingBasket.Create(Id);
    ShoppingBasket.AddItem(game, 1);
  }
}

