using GameStore.Application.Repositories;
using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Infrastructure.Repositories;

internal sealed class CustomerRepository(ApplicationDbContext dbContext) : ICustomerRepository
{
  public Task<Customer?> GetByIdAsync(CustomerId id, CancellationToken cancellationToken = default)
    => dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

  public Task<Customer?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
    => dbContext.Customers
      .Join(
        dbContext.Orders,
        customer => customer.Id,
        order => order.CustomerId,
        (customer, order) => new { customer, order })
      .Where(result => result.order.OrderNumber == orderNumber)
      .Select(result => result.customer)
      .FirstOrDefaultAsync(cancellationToken);

  public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
    => await dbContext.Customers.AddAsync(customer, cancellationToken);

  public Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
  {
    dbContext.Customers.Update(customer);
    return Task.CompletedTask;
  }
}
