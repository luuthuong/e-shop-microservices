using API.Requests.Products;
using Core.CQRS.Command;
using Core.CQRS.Query;
using Core.Infrastructure.Api;
using Microsoft.AspNetCore.Mvc;
using ProductSyncService.Application.Products.Commands;
using ProductSyncService.Application.Products.Queries;
using ProductSyncService.Domain.Products;

namespace API.Controllers;

public class ProductController : BaseController
{
    public ProductController(ICommandBus commandBus, IQueryBus queryBus) : base(commandBus, queryBus)
    {
    }

    [HttpPost("create")]
    public Task<IActionResult> Create(CreateProductRequest request)
    {
        return Response(
            CreateProduct.Create(
                request.Name,
                request.CategoryId,
                request.Description,
                request.ShortDescription
            )
        );
    }

    [HttpGet("products")]
    public Task<IActionResult> GetListProducts()
    {
        return Response(
            GetProducts.Create()
        );
    }

    [HttpGet("get/{id}")]
    public Task<IActionResult> GetById(Guid id)
    {
        return Response(
            GetProductById.Create(ProductId.From(id))
        );
    }

    [HttpPut("update/{id}")]
    public Task<IActionResult> UpdateProductByProductId(Guid id, [FromBody] UpdateProductRequest request)
    {
        return Response(
            UpdateProductById.Create(
                id,
                request.CategoryId,
                request.Name,
                request.Description,
                request.ShortDescription,
                request.Price
            )
        );
    }
    

    [HttpDelete("delete/{id}")]
    public Task<IActionResult> DeleteProduct()
    {
        return default;
    }
}