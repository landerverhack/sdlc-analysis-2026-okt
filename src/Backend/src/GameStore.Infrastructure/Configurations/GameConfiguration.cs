using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GameStore.Infrastructure.Configurations;

internal sealed class GameConfiguration : IEntityTypeConfiguration<Game>
{
  public void Configure(EntityTypeBuilder<Game> builder)
  {
    builder.ToTable("Games");
    builder.HasKey(g => g.Id);

    builder.Property(g => g.Id).ValueGeneratedOnAdd();
    builder.Property(g => g.Name).IsRequired().HasMaxLength(200);
    builder.Property(g => g.StockQuantity).IsRequired();

    builder.OwnsOne(g => g.Price, money =>
    {
      money.Property(m => m.Amount).HasColumnName("Price").HasPrecision(18, 2);
      money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3);
    });

    builder.Property(g => g.Image)
      .HasConversion(
        v => v.HasValue ? v.Value.ImageLocation : null,
        v => v != null ? new GameImage(v) : null)
      .HasColumnName("ImageURL")
      .HasMaxLength(500);

    builder.HasOne(g => g.Publisher)
      .WithMany()
      .HasForeignKey("PublisherId")
      .IsRequired()
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasIndex(g => g.Name).IsUnique();
  }
}

public class GameIdConverter : ValueConverter<GameId, int>
{
  public GameIdConverter() : base(v => v.Value, v => new GameId(v)) { }
}