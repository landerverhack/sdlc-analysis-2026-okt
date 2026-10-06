using GameStore.Application.Abstractions;
using GameStore.Application.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.Application.UseCases;

public interface IGetAllGamesUseCase : IUseCase
{
  Task<IReadOnlyList<Game>> GetAll(CancellationToken cancellationToken);
}

internal sealed class GetAllGamesUseCase(IGameRepository gameRepository) : IGetAllGamesUseCase
{
  public Task<IReadOnlyList<Game>> GetAll(CancellationToken cancellationToken)
    => gameRepository.GetAllAsync(cancellationToken);
}
