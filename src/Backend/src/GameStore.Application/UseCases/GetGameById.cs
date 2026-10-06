using GameStore.Application.Abstractions;
using GameStore.Application.Repositories;
using GameStore.Domain.Abstractions;
using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;

namespace GameStore.Application.UseCases;

public interface IGetGameByIdUseCase : IUseCase
{
  Task<Game> GetById(GameId gameId, CancellationToken cancellationToken);
}

internal sealed class GetGameByIdUseCase(IGameRepository gameRepository) : IGetGameByIdUseCase
{
  public async Task<Game> GetById(GameId gameId, CancellationToken cancellationToken)
  {
    var game = await gameRepository.GetByIdAsync(gameId, cancellationToken);
    if (game is null)
      throw new DomainException($"Game {gameId} not found");

    return game;
  }
}
