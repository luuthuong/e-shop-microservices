using Core.Domain;

namespace ProductSyncService.Domain.Quotes;

public class QuoteId: StronglyTypeId<Guid>
{
    private QuoteId(){}
    
    private QuoteId(Guid value) : base(value){}
    
    public static QuoteId From(Guid value) => new (value);

    public static IEnumerable<QuoteId> From(IList<Guid> values)
    {
        foreach (var item in values)
        {
            yield return From(item);
        }
    }
}