using Application.CQRS.Categories.Commands;
using Application.CQRS.Categories.Queries;
using Application.DTO;
using Core.BaseController;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

// [Authorize]
public class CategoryController: BaseController
{
    public CategoryController(IMediator mediator): base(mediator)
    {
    }
    [HttpPost]
    public async Task<IActionResult> Add(AddCategoryRequest request)
    {
        var result = await Mediator.Send(new AddCategoryCommand(request));
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var result = await Mediator.Send(new GetPagingCategoriesQuery());
        return Ok(result);
    }
    
    [HttpPost("publishProduct/{id}/{productId}")]
    public  Task<IActionResult>? PublishCategoryProduct(Guid id, Guid productId)
    {
        return default;
    }
}