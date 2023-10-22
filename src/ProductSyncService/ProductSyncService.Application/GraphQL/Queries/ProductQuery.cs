using GraphQL;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using ProductSyncService.Application.GraphQL.Types;
using ProductSyncService.Infrastructure.Database.Interfaces;

namespace ProductSyncService.Application.GraphQL.Queries;

public sealed class ProductQuery: ObjectGraphType
{
    public ProductQuery(IAppDbContext dbContext)
    {
        Field<ListGraphType<ProductType>>("products").ResolveAsync(async context => await dbContext.Product.ToListAsync());

        // Field<ProductType>("productById")
        //     .Argument<Guid>("id")
        //     .ResolveAsync(async context =>
        //     {
        //         Guid id = context.GetArgument<Guid>("id");
        //         return await dbContext.Product.FirstOrDefaultAsync(x => x.Id == id);
        //     });
    }

    public ProductQuery()
    {
    }
}