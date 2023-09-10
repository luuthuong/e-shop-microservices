using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController: ControllerBase
{
    protected readonly IMediator Mediator;

    protected BaseController(IMediator mediator)
    {
        Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
}