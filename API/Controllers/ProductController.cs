using Application.CQRS.Command.Products;
using Application.CQRS.Query.Products;
using Application.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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