using Core.Domain;

namespace Domain;

public sealed class QuoteId(Guid value) : StronglyTypeId<Guid>(value)
{
    public static QuoteId From(Guid value) => new QuoteId(value);
}