using Domain.CQRS.Command.Products;
using Domain.CQRS.Query.Products;
using Domain.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Controllers;

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