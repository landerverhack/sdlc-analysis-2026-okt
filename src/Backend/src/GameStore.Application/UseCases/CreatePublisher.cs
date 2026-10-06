using GameStore.Application.Abstractions;
using GameStore.Application.Repositories;
using GameStore.Domain.Abstractions;
using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;

namespace GameStore.Application.UseCases;

public interface ICreatePublisherUseCase : IUseCase
{
  Task<Publisher> CreatePublisher(string name, CancellationToken cancellationToken);
}

internal sealed class CreatePublisherUseCase(
    IPublisherRepository publisherRepository,
    IUnitOfWork unitOfWork) : ICreatePublisherUseCase
{
  public async Task<Publisher> CreatePublisher(string name, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(name))
      throw new DomainException("Publisher name is required");

    var publisher = new Publisher(new PublisherId(0), name);

    await publisherRepository.AddAsync(publisher, cancellationToken);
    await unitOfWork.CommitAsync(cancellationToken);

    return publisher;
  }
}
