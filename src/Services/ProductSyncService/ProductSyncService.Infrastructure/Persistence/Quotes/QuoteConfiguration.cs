using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductSyncService.Domain.Quotes;

namespace ProductSyncService.Infrastructure.Persistence.Quotes;

public class QuoteConfiguration: IEntityTypeConfiguration<Quote>
{
    public void Configure(EntityTypeBuilder<Quote> builder)
    {
        
    }
}