using Core.Domain;

namespace ProductSyncService.Domain.Quotes;

public sealed class CustomerId(Guid value) : StronglyTypeId<Guid>(value)
{
    public static CustomerId From(Guid value) => new CustomerId(value);
}
