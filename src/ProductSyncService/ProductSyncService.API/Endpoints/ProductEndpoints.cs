using API.Requests.Products;
using Core.Api;
using Core.Infrastructure.Api;
using ProductSyncService.Application.Products;
using ProductSyncService.Domain.Categories;
using ProductSyncService.Domain.Products;

namespace API.Endpoints;

internal sealed class ProductEndpoints(IServiceScopeFactory serviceScopeFactory)
    : AbstractApiEndpoint(serviceScopeFactory), IApiEndpoint
{
    public void Register(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", (ProductCreateRequest request) => ApiResponse(
                new CreateProductCommand(
                    request.Name,
                    request.Description,
                    request.ShortDescription,
                    CategoryId.From(request.CategoryId)
                )
            )
        );


        app.MapGet("/products", ([AsParameters] ProductGetListRequest request) => ApiResponse(
                new GetProductsQuery(
                    request.PageSize,
                    request.PageIndex,
                    request.Keyword,
                    request.OrderBy,
                    request.Descending
                )
            )
        );

        app.MapGet("/products/{id}", (Guid id) => ApiResponse(
                new GetProductByIdQuery(
                    ProductId.From(id)
                )
            )
        );

        app.MapDelete("/products/{id}", (Guid id) => ApiResponse(
                new DeleteProductCommand(
                    ProductId.From(id)
                )
            )
        );
    }
}