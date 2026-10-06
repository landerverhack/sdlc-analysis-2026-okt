namespace GameStore.Domain.ValueObjects;

/// <summary>
/// Order status enumeration
/// </summary>
public enum OrderStatus
{
    Placed = 1,
    Confirmed = 2,
    Shipped = 3,
    Cancelled = 4
}
