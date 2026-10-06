using GameStore.Application.Repositories;
using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Infrastructure.Repositories;

internal sealed class PublisherRepository(ApplicationDbContext dbContext) : IPublisherRepository
{
  public Task<Publisher?> GetByIdAsync(PublisherId id, CancellationToken cancellationToken = default)
    => dbContext.Publishers.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

  public async Task<IReadOnlyList<Publisher>> GetAllAsync(CancellationToken cancellationToken = default)
    => await dbContext.Publishers.ToListAsync(cancellationToken);

  public async Task AddAsync(Publisher publisher, CancellationToken cancellationToken = default)
    => await dbContext.Publishers.AddAsync(publisher, cancellationToken);

  public void Update(Publisher publisher)
    => dbContext.Publishers.Update(publisher);

  public Task<bool> ExistsAsync(PublisherId id, CancellationToken cancellationToken = default)
    => dbContext.Publishers.AnyAsync(p => p.Id == id, cancellationToken);
}
