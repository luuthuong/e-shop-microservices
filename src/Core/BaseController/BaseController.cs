﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.BaseController;

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