using GraphQL.Types;

namespace ProductSyncService.Application.GraphQL.Types;

public sealed class ProductKindType: ObjectGraphType<Domain.Entities.ProductType>
{
    public ProductKindType()
    {
        Field(x => x.Name);
        Field(x => x.Description);
        Field(x => x.CreatedDate);
        Field(x => x.UpdatedDate);
        Field(x => x.ParentProductTypeId);
    }
}