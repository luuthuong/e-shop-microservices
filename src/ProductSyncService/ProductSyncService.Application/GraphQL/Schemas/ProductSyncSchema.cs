using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using ProductSyncService.Application.GraphQL.Queries;
using ProductSyncService.Infrastructure.Database.Interfaces;

namespace ProductSyncService.Application.GraphQL.Schemas;
//
// public class ProductSyncSchema: Schema
// {
//     public ProductSyncSchema(
//         IServiceProvider services) : base(services)
//     {
//         var dbContext = services.GetRequiredService<IAppDbContext>();
//         Query = new ProductQuery(dbContext);
//     }
// }