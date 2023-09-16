using Application.CQRS.Command.Authentication;
using Domain.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AuthenticationController: BaseController
{
    public AuthenticationController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthRequest request)
    {
        var result = await Mediator.Send(new LoginCommand(request));
        return result.Data.IsAuthenticated ? Ok(result) : Unauthorized(result);
    }
}