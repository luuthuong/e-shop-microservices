using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using ProductSyncService.Domain.Entities;
using ProductSyncService.Infrastructure.Database.Interfaces;

namespace ProductSyncService.Application.GraphQL.Types;

public sealed class ProductType: ObjectGraphType<Product>
{
    public ProductType()
    {
        Field(x => x.Name);
        Field(x => x.Description);
        Field(x => x.ShortDescription);
        Field(x => x.Price);
        Field(x => x.ProductTypeId);
        Field(x => x.Published);
        Field(x => x.Quantity);
        Field(x => x.CreatedDate);
        Field(x => x.UpdatedDate);
        // Field<Domain.Entities.ProductType>("ProductType").ResolveAsync(async context =>
        //     await dbContext.ProductType.FirstOrDefaultAsync(x => x.Id == context.Source.ProductTypeId));
    }
}