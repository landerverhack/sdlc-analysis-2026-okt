using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;

namespace GameStore.Application.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(CustomerId id, CancellationToken cancellationToken = default);
    Task<Customer?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    Task AddAsync(Customer customer, CancellationToken cancellationToken = default);
    Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default);
}
