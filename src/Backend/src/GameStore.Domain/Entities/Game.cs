using GameStore.Domain.ValueObjects;

namespace GameStore.Domain.Entities;

[DebuggerDisplay("Game {Name} - {Price.Amount}.")]
public sealed class Game : IEntity<GameId>
{
  private const string DefaultImageURL = "https://u2ublogimages.blob.core.windows.net/cleanarchitecture/GamesStore_BoardGame.jpg";

  private readonly static Money DefaultGamePrice = new Money(50);

  /// <summary>
  /// Ctor for use by EF Core // TODO
  /// </summary>
  /// <param name="id">primary key</param>
  /// <param name="name">name</param>
  internal Game(GameId id, string name)
  {
    Id = id;
    Name = name;
  }

  internal Game(GameId id, string name, Publisher publisher) : this(id, name)
  {
    SetPrice(DefaultGamePrice);
    Publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
    Publisher.AddGame(this);
  }

  public GameId Id { get; private set; }
  public string Name { get; private set; }
  public Publisher Publisher { get; private set; }
  public Money Price { get; private set; }
  public int StockQuantity { get; private set; }
  public GameImage? Image { get; private set; }

  public string ImageURL
  {
    get
    {
      if (Image.HasValue)
      {
        return Image.Value.ImageLocation;
      }
      return DefaultImageURL;
    }
  }

  public string PublisherName
    => Publisher.Name;


  public void Rename(string name)
  {
    if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));

    Name = name;
  }

  public void SetPrice(in Money price)
  {
    if (price.Amount <= 0) throw new ArgumentException(nameof(price));
    Price = price;
  }

  public void SetImage(string imageUrl)
  {
    var image = new GameImage(imageUrl);
    Image = image;
  }

  public void ChangePublisher(Publisher pub)
  {
    if (Publisher == pub)
      return;
    Publisher.RemoveGame(this);
    pub.AddGame(this);
    Publisher = pub;
  }

  public void SetStock(int quantity)
  {
    if (quantity < 0)
      throw new ArgumentException("Stock cannot be negative", nameof(quantity));

    StockQuantity = quantity;
  }

  public void DecreaseStock(int quantity)
  {
    if (quantity <= 0)
      throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

    if (StockQuantity < quantity)
      throw new DomainException($"Insufficient stock for game {Id}. Requested {quantity}, available {StockQuantity}");

    StockQuantity -= quantity;
  }
}
