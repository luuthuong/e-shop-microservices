using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using ProductSyncService.Application.GraphQL.Queries;

namespace ProductSyncService.Application.GraphQL.Schemas;

public class ProductSyncTypeSchema: Schema
{
    public ProductSyncTypeSchema(IServiceProvider services) : base(services)
    {
        Query = services.GetRequiredService<ProductTypeQuery>();
    }
}