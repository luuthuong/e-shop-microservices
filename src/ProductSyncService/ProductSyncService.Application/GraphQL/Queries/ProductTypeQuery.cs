using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using ProductSyncService.Application.GraphQL.Types;
using ProductSyncService.Infrastructure.Database.Interfaces;

namespace ProductSyncService.Application.GraphQL.Queries;

public sealed class ProductTypeQuery: ObjectGraphType
{
    public ProductTypeQuery()
    {
        Field<ListGraphType<ProductKindType>>("ProductTypes")
            .Resolve(x => Enumerable.Empty<Domain.Entities.ProductType>());
    }
}