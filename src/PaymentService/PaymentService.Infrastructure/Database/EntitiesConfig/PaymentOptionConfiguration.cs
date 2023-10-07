using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntitiesConfig;

public class PaymentOptionConfiguration: IEntityTypeConfiguration<PaymentOption>
{
    public void Configure(EntityTypeBuilder<PaymentOption> builder)
    {
    }
}