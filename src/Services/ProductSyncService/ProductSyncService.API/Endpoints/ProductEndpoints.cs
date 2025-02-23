using API.Requests.Products;
using Core.Api;
using Core.Infrastructure.Api;
using ProductSyncService.Application.Products;
using ProductSyncService.Domain.Categories;
using ProductSyncService.Domain.Products;

namespace API.Endpoints;

internal sealed class ProductEndpoints(IServiceScopeFactory serviceScopeFactory)
    : AbstractApiEndpoint(serviceScopeFactory)
{
    public override void Register(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", CreateProduct).RequireAuthorization();

        app.MapGet("/products", GetProducts);

        app.MapGet("/products/{id}", GetProductById);

        app.MapDelete("/products/{id}", DeleteProduct);
    }

    private Task<IResult> CreateProduct(ProductCreateRequest request) => ApiResponse(
        new CreateProductCommand(
            request.Name,
            request.Description,
            request.ShortDescription,
            CategoryId.From(request.CategoryId)
        )
    );

    private Task<IResult> GetProducts([AsParameters] ProductGetListRequest request) => ApiResponse(
        new GetProductsQuery(
            request.PageSize,
            request.PageIndex,
            request.Keyword,
            request.OrderBy,
            request.Descending
        )
    );

    private Task<IResult> GetProductById(Guid id) => ApiResponse(
        new GetProductByIdQuery(
            ProductId.From(id)
        )
    );

    private Task<IResult> DeleteProduct(Guid id) => ApiResponse(
        new DeleteProductCommand(
            ProductId.From(id)
        )
    );
}