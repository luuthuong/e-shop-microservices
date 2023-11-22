using System.Net;
using Core.CQRS.Command;
using Core.CQRS.Query;
using Core.Results;
using Microsoft.AspNetCore.Mvc;

namespace Core.Infrastructure.Api;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController: ControllerBase
{
    private readonly ICommandBus _commandBus;
    private readonly IQueryBus _queryBus;

    protected BaseController(ICommandBus commandBus, IQueryBus queryBus)
    {
        _commandBus = commandBus ?? throw new ArgumentNullException(nameof(commandBus));
        _queryBus = queryBus ?? throw new ArgumentNullException(nameof(queryBus));
    }

    protected new async Task<IActionResult> Response<TResult>(IQuery<TResult> query)
    {
        TResult result;
        try
        {
            result = await _queryBus.SendAsync(query);
        }
        catch (System.Exception e)
        {
            return BadRequest(new ApiResponse<TResult>()
            {
                Status = Result.Failure(new Error(HttpStatusCode.BadRequest.ToString(), e.Message))
            });
        }

        return Ok(new ApiResponse<TResult>()
        {
            Status = Result.Success(),
            Data = result
        });
    }

    protected new async Task<IActionResult> Response(ICommand command)
    {
        try
        {
            await _commandBus.SendAsync(command);
        }
        catch (System.Exception e)
        {
            return BadRequest(new ApiResponse<IActionResult>()
            {
                Status = Result.Failure(new Error(HttpStatusCode.BadRequest.ToString(), e.Message))
            });
        }

        return Ok(new ApiResponse<IActionResult>()
        {
            Status = Result.Success()
        });
    }
}