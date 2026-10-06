using GameStore.Domain.ValueObjects;

namespace GameStore.Domain.Entities;

public sealed class Publisher : IEntity<PublisherId>
{
  public Publisher(PublisherId id, string name)
  {
    Id = id;
    Name = name;
  }

  public PublisherId Id { get;private set; }
  public string Name { get; private set; }

  private List<Game>? games;
  public IEnumerable<Game> Games => GetGamesList();


  private List<Game> GetGamesList()
    => games ??= [];

  internal void AddGame(Game g)
    => GetGamesList().Add(g);

  internal void RemoveGame(Game g)
    => GetGamesList().Remove(g);

  public Game CreateGame(string name, GameId id) // TODO pass gemid here or set to 0 and let the repository set it when saving?
    => new Game(id, name, this);
}