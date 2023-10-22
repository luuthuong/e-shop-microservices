using Core.BaseController;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductSyncService.Application.CQRS.ProductTypes.Commands;
using ProductSyncService.Application.CQRS.ProductTypes.Queries;
using ProductSyncService.Application.DTO;

namespace API.Controllers;

public class ProductTypeController: BaseController
{
    public ProductTypeController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductTypeRequest request)
    {
        var result = await Mediator.Send(new CreateProductTypeCommand(
            request.Name, request.ParentId, request.Description));
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllProductTypeQuery());
        return Ok(result);
    }
}