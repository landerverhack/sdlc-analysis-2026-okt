using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GameStore.Infrastructure.Configurations;

internal sealed class ShoppingBasketConfiguration : IEntityTypeConfiguration<ShoppingBasket>
{
  public void Configure(EntityTypeBuilder<ShoppingBasket> builder)
  {
    builder.ToTable("ShoppingBaskets");
    builder.HasKey(e => e.Id);

    builder.Property(e => e.Id).ValueGeneratedOnAdd();
    builder.Property(e => e.CustomerId).IsRequired();

    builder.OwnsOne(e => e.Subtotal, money =>
    {
      money.Property(m => m.Amount).HasColumnName("Subtotal").HasPrecision(18, 2);
      money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3);
    });

    builder.HasMany(e => e.Lines)
      .WithOne()
      .HasForeignKey("ShoppingBasketId")
      .IsRequired()
      .OnDelete(DeleteBehavior.Cascade);

    IMutableNavigation? linesNav = builder.Metadata.FindNavigation(nameof(ShoppingBasket.Lines));
    linesNav!.SetPropertyAccessMode(PropertyAccessMode.Field);

    builder.HasIndex(e => e.CustomerId).IsUnique();
  }
}

public class ShoppingBasketIdConverter : ValueConverter<ShoppingBasketId, int>
{
  public ShoppingBasketIdConverter() : base(v => v.Value, v => new ShoppingBasketId(v)) { }
}

public class BasketLineIdConverter : ValueConverter<BasketLineId, int>
{
  public BasketLineIdConverter() : base(v => v.Value, v => new BasketLineId(v)) { }
}