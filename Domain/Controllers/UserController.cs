using Domain.CQRS.Command.Users;
using Domain.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Controllers;

public class UserController: BaseController
{
    public UserController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(AddUserRequest request)
    {
        var result = await Mediator.Send(new AddUserCommand(request));
        return Ok(result);
    }
}