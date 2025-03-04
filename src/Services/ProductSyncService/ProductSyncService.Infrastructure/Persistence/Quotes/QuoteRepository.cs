using Core.Infrastructure.EF.Repository;
using ProductSyncService.Domain.Quotes;

namespace ProductSyncService.Infrastructure.Persistence.Quotes;

public sealed class QuoteRepository(ProductSyncDbContext dbContext) : Repository<ProductSyncDbContext, Quote, QuoteId>(dbContext), IQuoteRepository
{
    
}