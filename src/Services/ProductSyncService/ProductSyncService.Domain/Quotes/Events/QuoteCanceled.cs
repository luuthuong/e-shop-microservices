using Core.Domain;

namespace ProductSyncService.Domain.Quotes.Events;

public record class QuoteCanceled : IDomainEvent
{
    public Guid QuoteId { get; private set; }

    public static QuoteCanceled Create(Guid quoteId)
    {
        if (quoteId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(quoteId));
       
        return new QuoteCanceled(quoteId);
    }

    private QuoteCanceled(Guid quoteId) => QuoteId = quoteId;
}