namespace GameStore.Domain.ValueObjects;

[DebuggerDisplay("Money: {Amount} {Currency}")]
public sealed record Money
{
  public static Money Zero { get; } = new Money(0, CurrencyName.EUR);

  public static Money Eur(decimal amount) => new Money(amount, CurrencyName.EUR);

  public decimal Amount { get; }
  public CurrencyName Currency { get; }

  public Money(decimal amount, CurrencyName currency)
  {
    Amount = amount;
    Currency = currency;
  }

  public Money(decimal amount) : this(amount, CurrencyName.EUR) { }

  public Money(decimal amount, string currency)
  : this(amount, (CurrencyName)Enum.Parse(typeof(CurrencyName), currency)) { }


  //[Ignore] // TODO
  public Money Rounded
  {
    get
    {
      decimal amount = Amount * 100 + 49;
      int rounded = (int)amount;
      rounded = rounded - (rounded % 50);
      amount = (decimal)(rounded - 1) / 100;
      return new Money(amount, Currency);
    }
  }

  public override string ToString()
  => string.Format(FormatStringFor(Currency), Amount.ToString());

  private static string FormatStringFor(CurrencyName currency)
  {
    return currency switch
    {
      CurrencyName.USD => "${0}",
      CurrencyName.EUR => "{0}€",
      CurrencyName.JPY => "{0}¥",
      _ => $"{{0}} {currency}",
    };
  }

  // Simplified version that only adds two amounts with the same currency
  public static Money operator +(Money m1, Money m2)
  {
    Debug.Assert(m1.Currency == m2.Currency);
    return new Money(m1.Amount + m2.Amount, m1.Currency);
  }

  public static Money operator -(Money m1, Money m2)
  {
    Debug.Assert(m1.Currency == m2.Currency);
    return new Money(m1.Amount - m2.Amount, m1.Currency);
  }
}

