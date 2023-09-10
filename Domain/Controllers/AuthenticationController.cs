using Domain.Core;
using Domain.CQRS.Command.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Controllers;

public class AuthenticationController: BaseController
{
    public AuthenticationController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthRequest request)
    {
        var result = await Mediator.Send(new LoginCommand(request));
        return result.IsAuthenticated ? Ok(result) : Unauthorized(result);
    }
}