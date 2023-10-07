using Core.BaseController;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductSyncService.Application.CQRS.Products.Commands;
using ProductSyncService.Application.CQRS.Products.Queries;
using ProductSyncService.Application.DTO;

namespace API.Controllers;

public class ProductController: BaseController
{
    public ProductController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpGet]
    public async Task<IActionResult> Gets()
    {
        var results = await Mediator.Send(new GetProductsQuery());
        return Ok(results);
    } 

    [HttpPost]
    public async Task<IActionResult> Add(AddProductRequest request)
    {
        var result = await Mediator.Send(new AddProductCommand(request));
        return Ok(result);
    }
}