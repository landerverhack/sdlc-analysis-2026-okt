using GameStore.Application.Repositories;
using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Infrastructure.Repositories;

internal sealed class OrderRepository(ApplicationDbContext dbContext) : IOrderRepository
{
  public Task<Order?> GetByIdAsync(OrderId id, CancellationToken cancellationToken = default)
    => dbContext.Orders
      .Include(o => o.OrderLines)
      .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

  public Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
    => dbContext.Orders
      .Include(o => o.OrderLines)
      .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, cancellationToken);

  public async Task<IReadOnlyList<Order>> GetByCustomerIdAsync(CustomerId customerId, CancellationToken cancellationToken = default)
    => await dbContext.Orders
      .Include(o => o.OrderLines)
      .Where(o => o.CustomerId == customerId)
      .ToListAsync(cancellationToken);

  public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    => await dbContext.Orders.AddAsync(order, cancellationToken);

  public Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
  {
    dbContext.Orders.Update(order);
    return Task.CompletedTask;
  }
}
