using GameStore.Application.Abstractions;
using GameStore.Application.Repositories;
using GameStore.Domain.Abstractions;
using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;

namespace GameStore.Application.UseCases;

public interface ICreateGameUseCase : IUseCase
{
  Task<Game> CreateGame(
      string title,
      decimal price,
      CurrencyName currency,
      int stockQuantity,
      PublisherId publisherId,
      string? imageUrl,
      CancellationToken cancellationToken);
}

internal sealed class CreateGameUseCase(
    IGameRepository gameRepository,
    IPublisherRepository publisherRepository,
    IUnitOfWork unitOfWork) : ICreateGameUseCase
{
  public async Task<Game> CreateGame(
      string title,
      decimal price,
      CurrencyName currency,
      int stockQuantity,
      PublisherId publisherId,
      string? imageUrl,
      CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(title))
      throw new DomainException("Game title is required");

    var publisher = await publisherRepository.GetByIdAsync(publisherId, cancellationToken);
    if (publisher is null)
      throw new DomainException($"Publisher {publisherId} not found");

    var game = publisher.CreateGame(title, new GameId(0));
    game.SetPrice(new Money(price, currency));
    game.SetStock(stockQuantity);

    if (!string.IsNullOrWhiteSpace(imageUrl))
      game.SetImage(imageUrl);

    await gameRepository.AddAsync(game, cancellationToken);
    await unitOfWork.CommitAsync(cancellationToken);

    return game;
  }
}
