using Application.CQRS.Command.Categories;
using Application.CQRS.Query.Categories;
using Application.DTO;
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
}