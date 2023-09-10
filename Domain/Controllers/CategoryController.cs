using Domain.CQRS.Command.Categories;
using Domain.CQRS.Query.Categories;
using Domain.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Controllers;

[Authorize]
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