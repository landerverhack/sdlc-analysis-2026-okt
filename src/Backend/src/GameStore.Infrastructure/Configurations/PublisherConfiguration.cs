using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GameStore.Infrastructure.Configurations;

internal sealed class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
{
  public void Configure(EntityTypeBuilder<Publisher> builder)
  {
    builder.ToTable("Publishers");
    builder.HasKey(p => p.Id);
    builder.HasAlternateKey(p => p.Name);
    builder.Property(p => p.Id).ValueGeneratedOnAdd();
    builder.Property(p => p.Name).IsRequired().HasMaxLength(200);

    builder.HasMany(p => p.Games)
      .WithOne(g => g.Publisher)
      .IsRequired();

    // Use the games field instead of the property, because the property has side-effects
    IMutableNavigation? gamesNav = builder.Metadata
                          .FindNavigation(nameof(Publisher.Games));
    gamesNav!.SetPropertyAccessMode(PropertyAccessMode.Field);
  }
}

public class PublisherIdConverter : ValueConverter<PublisherId, int>
{
  public PublisherIdConverter() : base(v => v.Value, v => new PublisherId(v)) { }
}