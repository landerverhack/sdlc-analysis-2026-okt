using GameStore.Application.Abstractions;

namespace GameStore.Infrastructure.Repositories;

internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
  public Task<int> CommitAsync(CancellationToken cancellationToken = default)
    => dbContext.SaveChangesAsync(cancellationToken);
}
