using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Infrastructure.Configurations;

internal sealed class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
{
  public void Configure(EntityTypeBuilder<OrderLine> builder)
  {
    builder.ToTable("OrderLines");
    builder.HasKey(l => l.Id);

    builder.Property(l => l.Id)
      .HasConversion<OrderLineIdConverter>()
      .ValueGeneratedOnAdd();

    builder.Property<OrderId>("OrderId")
      .HasConversion<OrderIdConverter>()
      .IsRequired();

    builder.Property(l => l.GameId).IsRequired();
    builder.Property(l => l.GameName).IsRequired().HasMaxLength(200);
    builder.Property(l => l.Quantity).IsRequired();

    builder.OwnsOne(l => l.UnitPrice, money =>
    {
      money.Property(m => m.Amount).HasColumnName("UnitPrice").HasPrecision(18, 2);
      money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3);
    });
  }
}
