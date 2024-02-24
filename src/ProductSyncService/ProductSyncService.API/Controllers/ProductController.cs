using API.Requests.Products;
using Core.CQRS.Command;
using Core.CQRS.Query;
using Core.Infrastructure.Api;
using Microsoft.AspNetCore.Mvc;
using ProductSyncService.Application.Products.Commands;
using ProductSyncService.Application.Products.Queries;
using ProductSyncService.Domain.Products;

namespace API.Controllers;

public class ProductController(ICommandBus commandBus, IQueryBus queryBus) : BaseController(commandBus, queryBus)
{
    [HttpPost("create")]
    public Task<IActionResult> Create(CreateProductRequest request)
    {
        return ApiResponse(
            CreateProduct.Create(
                request.Name,
                request.CategoryId,
                request.Description,
                request.ShortDescription
            )
        );
    }

    [HttpGet("gets")]
    public Task<IActionResult> GetListProducts()
    {
        return ApiResponse(
            GetProducts.Create()
        );
    }

    [HttpGet("get/{id}")]
    public Task<IActionResult> GetById(Guid id)
    {
        return ApiResponse(
            GetProductById.Create(ProductId.From(id))
        );
    }

    [HttpPut("update/{id}")]
    public Task<IActionResult> UpdateProductByProductId(Guid id, [FromBody] UpdateProductRequest request)
    {
        return ApiResponse(
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