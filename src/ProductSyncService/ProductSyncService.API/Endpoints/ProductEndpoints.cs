using API.Requests.Products;
using Core.Api;
using Core.CQRS.Command;
using Core.CQRS.Query;
using Microsoft.AspNetCore.Mvc;
using ProductSyncService.Application.Products.Commands;
using ProductSyncService.Application.Products.Queries;
using ProductSyncService.Domain.Categories;
using ProductSyncService.Domain.Products;

namespace API.Endpoints;

internal sealed class ProductEndpoints: IApiEndpoint
{
    public void Register(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/products", async (ICommandBus commandBus, ProductCreateRequest request) =>
        {
            await commandBus.SendAsync(
                new CreateProductCommand(
                    request.Name,
                    request.Description,
                    request.ShortDescription,
                    CategoryId.From(request.CategoryId)
                )
            );
            return Results.Ok();
        });
        
        app.MapGet("/api/products", async ([AsParameters]ProductGetRequest request, IQueryBus queryBus) =>
        {
            var result = await queryBus.SendAsync(
                new GetProductsQuery(
                    request.PageSize,
                    request.PageIndex,
                    request.Keyword,
                    request.OrderBy,
                    request.Descending
                )
            );

            return Results.Ok(result);
        });
        
        app.MapGet("api/products/{id}", async (Guid id, IQueryBus queryBus) =>
        {
            var result = await queryBus.SendAsync(
                new GetProductByIdQuery(
                    ProductId.From(id)
                )
            );
            return Results.Ok(result);
        });

        app.MapDelete("api/products/{id}", async (Guid id, ICommandBus commandBus) =>
        {
            
        });
    }
}