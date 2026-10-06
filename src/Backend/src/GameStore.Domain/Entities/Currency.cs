using GameStore.Domain.ValueObjects;

namespace GameStore.Domain.Entities;

/// <summary>
/// Currency with its current exchange rate in EUR
/// </summary>
[DebuggerDisplay("{Name,nq} = {ValueInEuro}EUR")]
public sealed class Currency : IEntity<CurrencyId>
{
  public Currency(CurrencyId id, CurrencyName name, decimal valueInEuro)
  {
    Id = id;
    Name = name;
    ValueInEuro = valueInEuro;
  }
  public CurrencyId Id { get;private set; }
  public CurrencyName Name { get; }
  public decimal ValueInEuro { get; }

  public static CurrencyName Parse(string currencyAsString)
    => Enum.Parse<CurrencyName>(currencyAsString);
}
