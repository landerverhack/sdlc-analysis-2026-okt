using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GameStore.Infrastructure.Configurations;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
  public void Configure(EntityTypeBuilder<Order> builder)
  {
    builder.ToTable("Orders");
    builder.HasKey(e => e.Id);

    builder.Property(e => e.Id).ValueGeneratedOnAdd();
    builder.Property(e => e.OrderNumber).IsRequired().HasMaxLength(50);
    builder.Property(e => e.CustomerId).IsRequired();
    builder.Property(e => e.CustomerEmail).IsRequired().HasMaxLength(255);
    builder.Property(e => e.OrderDate).IsRequired();
    builder.Property(e => e.Status)
          .IsRequired()
          .HasConversion<string>()
          .HasMaxLength(20);

    builder.OwnsOne(e => e.Subtotal, money =>
    {
      money.Property(m => m.Amount).HasColumnName("SubtotalAmount").HasPrecision(18, 2);
      money.Property(m => m.Currency).HasColumnName("SubtotalCurrency").HasMaxLength(3);
    });

    builder.OwnsOne(e => e.Discount, money =>
    {
      money.Property(m => m.Amount).HasColumnName("DiscountAmount").HasPrecision(18, 2);
      money.Property(m => m.Currency).HasColumnName("DiscountCurrency").HasMaxLength(3);
    });

    builder.OwnsOne(e => e.FinalTotal, money =>
    {
      money.Property(m => m.Amount).HasColumnName("FinalTotalAmount").HasPrecision(18, 2);
      money.Property(m => m.Currency).HasColumnName("FinalTotalCurrency").HasMaxLength(3);
    });

    builder.HasMany(e => e.OrderLines)
      .WithOne()
      .HasForeignKey("OrderId")
      .IsRequired()
      .OnDelete(DeleteBehavior.Cascade);

    IMutableNavigation? orderLinesNav = builder.Metadata.FindNavigation(nameof(Order.OrderLines));
    orderLinesNav!.SetPropertyAccessMode(PropertyAccessMode.Field);

    builder.HasIndex(e => e.OrderNumber).IsUnique();
    builder.HasIndex(e => e.CustomerId);
  }
}


public class OrderIdConverter : ValueConverter<OrderId, int>
{
  public OrderIdConverter() : base(v => v.Value, v => new OrderId(v)) { }
}

public class OrderLineIdConverter : ValueConverter<OrderLineId, int>
{
  public OrderLineIdConverter() : base(v => v.Value, v => new OrderLineId(v)) { }
}