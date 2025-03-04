using Core.EF;

namespace ProductSyncService.Domain.Quotes;

public interface IQuoteRepository: IRepository<Quote, QuoteId>
{
    
}