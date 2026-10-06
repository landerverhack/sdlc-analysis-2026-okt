using GameStore.Application.Abstractions;
using GameStore.Application.Repositories;
using GameStore.Domain.Abstractions;
using GameStore.Domain.Customers;
using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;

namespace GameStore.Application.UseCases;

public interface ICreateCustomerUseCase : IUseCase
{
  Task<Customer> CreateCustomer(
      string email,
      string firstName,
      string lastName,
      Address address,
      bool isVip,
      CancellationToken cancellationToken);
}

internal sealed class CreateCustomerUseCase(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork) : ICreateCustomerUseCase
{
  public async Task<Customer> CreateCustomer(
      string email,
      string firstName,
      string lastName,
      Address address,
      bool isVip,
      CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(firstName))
      throw new DomainException("First name is required");

    if (string.IsNullOrWhiteSpace(lastName))
      throw new DomainException("Last name is required");

    var customer = new Customer(new CustomerId(0), firstName, lastName)
    {
      Email = new EmailAddress(email),
    };

    customer.MoveToNewAddress(address);
  customer.SetVipStatus(isVip);

    await customerRepository.AddAsync(customer, cancellationToken);
    await unitOfWork.CommitAsync(cancellationToken);

    return customer;
  }
}
