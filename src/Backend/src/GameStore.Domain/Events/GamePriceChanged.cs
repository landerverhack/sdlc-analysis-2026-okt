using GameStore.Domain.ValueObjects;

namespace GameStore.Domain.Events;

public sealed record GamePriceChanged(GameId GameId, Money OldPrice, Money NewPrice) : IDomainEvent;
