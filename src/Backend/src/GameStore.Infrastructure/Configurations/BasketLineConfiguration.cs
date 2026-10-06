using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Infrastructure.Configurations;

internal sealed class BasketLineConfiguration : IEntityTypeConfiguration<BasketLine>
{
  public void Configure(EntityTypeBuilder<BasketLine> builder)
  {
    builder.ToTable("BasketLines");
    builder.HasKey(l => l.Id);

    builder.Property(l => l.Id)
      .HasConversion<BasketLineIdConverter>()
      .ValueGeneratedOnAdd();

    builder.Property<ShoppingBasketId>("ShoppingBasketId")
      .HasConversion<ShoppingBasketIdConverter>()
      .IsRequired();

    builder.Property(l => l.GameId).IsRequired();
    builder.Property(l => l.Quantity).IsRequired();

    builder.HasOne(l => l.Game)
      .WithMany()
      .HasForeignKey(l => l.GameId)
      .IsRequired()
      .OnDelete(DeleteBehavior.Restrict);
  }
}
