using Domain.CQRS.Command.Users;
using Domain.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Controllers;

public class UserController: BaseController
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(AddUserRequest request)
    {
        var result = await _mediator.Send(new AddUserCommand(request));
        return Ok(result);
    }
}