using System.Text.RegularExpressions;

namespace GameStore.Domain.Customers;

public sealed partial record EmailAddress
{
  public static readonly EmailAddress Empty = new();

  public string Value { get; }

  private EmailAddress()
  {
    Value = string.Empty;
  }

  public EmailAddress(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
      throw new ArgumentException("Email cannot be empty.", nameof(value));

    if (!EmailRegex.IsMatch(value))
      throw new ArgumentException("Invalid email format.", nameof(value));

    Value = value.ToLowerInvariant();
  }

  public override string ToString() => Value;
  public bool IsEmpty() => Equals(this, Empty);

  [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
  private static partial Regex EmailRegex { get; }
}

