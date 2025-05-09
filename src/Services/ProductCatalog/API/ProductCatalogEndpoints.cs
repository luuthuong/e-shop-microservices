using Core.Infrastructure.Api;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.Commands.CreateProduct;
using ProductCatalog.Application.Commands.ReserveStockCommand;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Queries.GetProduct;
using ProductCatalog.Application.Queries.GetProducts;

namespace ProductCatalog.API;

public class ProductCatalogEndpoints(IServiceScopeFactory serviceScopeFactory)
    : AbstractApiEndpoint(serviceScopeFactory)
{
    public override string GroupName => "ProductCatalog";

    public override void Register(IEndpointRouteBuilder route)
    {
        route.MapPost("/", ([FromBody] CreateProductCommand request) => ApiResponse(request))
            .WithName("CreateProduct")
            .Produces<bool>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(GroupName);

        route.MapPost("/reserve", ([FromBody] ReserveStockCommand request) => ApiResponse(request))
            .WithName("ReserveProduct")
            .Produces<bool>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(GroupName);

        route.MapGet("/{id:guid}", ([FromRoute] Guid id) => ApiResponse(new GetProductQuery(id)))
            .WithName("GetProduct")
            .Produces<ProductDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(GroupName);

        route.MapGet("/", () => ApiResponse(new GetProductsQuery()))
            .WithName("GetProducts")
            .Produces<List<ProductDTO>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(GroupName);
    }
}