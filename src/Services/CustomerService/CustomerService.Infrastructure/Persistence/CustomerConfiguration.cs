using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerService.Infrastructure.Persistence;

public class CustomerConfiguration: IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        
        builder.HasKey(c => c.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                c => c.Value,
                v => CustomerId.From(v)
            );
        
        builder.OwnsOne(
            e => e.Address,
            b =>
            {
                b.Property(x => x.StreetAddress).HasColumnName("StreetAddress");
            }
        );

        builder.OwnsOne(e => e.CreditLimit, b =>
        {
            b.Property(x => x.Amount).HasColumnName("CreditLimit").HasColumnType("decimal(18, 2)");
        });
    }
}