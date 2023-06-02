using Domain.CQRS.Command.Products;
using Domain.CQRS.Query.Products;
using Domain.DTO;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController: ControllerBase
{
    private readonly IMediator _mediator;
    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Gets()
    {
        var results = await _mediator.Send(new GetProductsQuery());
        return Ok(results);
    } 

    [HttpPost]
    public async Task<IActionResult> Add(AddProductRequest request)
    {
        var result = await _mediator.Send(new AddProductCommand
        {
            Request = request
        });
        return Ok(result);
    }
}