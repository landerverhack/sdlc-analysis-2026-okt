using GameStore.Domain.Customers;
using GameStore.Domain.Entities;
using GameStore.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GameStore.Infrastructure.Configurations;

internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
  public void Configure(EntityTypeBuilder<Customer> builder)
  {
    builder.ToTable("Customers");
    builder.HasKey(c => c.Id);
    builder.Property(c => c.Id).ValueGeneratedOnAdd();
    builder.Property(c => c.Email).IsRequired();
    builder.Property(c => c.FirstName).IsRequired().HasMaxLength(100);
    builder.Property(c => c.LastName).IsRequired().HasMaxLength(100);
    builder.Property(c => c.IsVip).IsRequired();

    builder.HasIndex(c => c.Email).IsUnique();

    builder.OwnsOne(c => c.Address, address =>
    {
      address.Property(a => a.Street).HasMaxLength(200);
      address.Property(a => a.City).HasMaxLength(100);
    });

    builder.HasOne(c => c.ShoppingBasket)
      .WithOne()
      .HasForeignKey<ShoppingBasket>(sb => sb.CustomerId)
      .IsRequired(false);
  }
}


public class CustomerIdConverter : ValueConverter<CustomerId, int>
{
  public CustomerIdConverter() : base(v => v.Value, v => new CustomerId(v)) { }
}

public class EmailAddressConverter : ValueConverter<EmailAddress, string>
{
  public EmailAddressConverter() : base(v => v.Value, v => new EmailAddress(v)) { }
}