namespace GameStore.Domain.Abstractions
{
  /// <summary>
  /// IEntity is used for enforcing generic constraints.
  /// </summary>
  public interface IEntity<out Tid>
  {
    Tid Id { get; }
  }
}
