using GameStore.Application.Abstractions;
using GameStore.Application.Repositories;
using GameStore.Domain.Abstractions;
using GameStore.Domain.Entities;
using GameStore.Domain.Events;
using GameStore.Domain.ValueObjects;

namespace GameStore.Application.UseCases;

public interface IUpdateGameUseCase : IUseCase
{
  Task<Game> UpdateGame(
      GameId gameId,
      string title,
      decimal price,
      CurrencyName currency,
      PublisherId publisherId,
      string? imageUrl,
      CancellationToken cancellationToken);
}

internal sealed class UpdateGameUseCase(
    IGameRepository gameRepository,
    IPublisherRepository publisherRepository,
    IDomainEventPublisher publisher,
    IUnitOfWork unitOfWork) : IUpdateGameUseCase
{
  public async Task<Game> UpdateGame(
      GameId gameId,
      string title,
      decimal price,
      CurrencyName currency,
      PublisherId publisherId,
      string? imageUrl,
      CancellationToken cancellationToken)
  {
    var game = await gameRepository.GetByIdAsync(gameId, cancellationToken);
    if (game is null)
      throw new DomainException($"Game {gameId} not found");

    var targetPublisher = await publisherRepository.GetByIdAsync(publisherId, cancellationToken);
    if (targetPublisher is null)
      throw new DomainException($"Publisher {publisherId} not found");

    var oldPrice = game.Price;
    var newPrice = new Money(price, currency);

    game.Rename(title);
    game.SetPrice(newPrice);

    if (!string.IsNullOrWhiteSpace(imageUrl))
      game.SetImage(imageUrl);

    if (game.Publisher.Id != targetPublisher.Id)
      game.ChangePublisher(targetPublisher);

    gameRepository.Update(game);
    await unitOfWork.CommitAsync(cancellationToken);

    if (oldPrice != newPrice)
      await publisher.PublishAsync(new GamePriceChanged(game.Id, oldPrice, newPrice), cancellationToken);

    return game;
  }
}
