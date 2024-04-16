using System.Net;
using Core.Api;
using Core.CQRS.Command;
using Core.CQRS.Query;
using Core.Results;
using Microsoft.AspNetCore.Mvc;

namespace Core.Infrastructure.Api;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController(ICommandBus commandBus, IQueryBus queryBus) : ControllerBase
{
    protected async Task<IActionResult> ApiResponse<TResult>(IQuery<TResult> query)
    {
        TResult result;
        try
        {
            result = await queryBus.SendAsync(query);
        }
        catch (System.Exception e)
        {
            return BadRequest(
                new ApiResponse<TResult>(
                    Result.Failure(
                        new Error(HttpStatusCode.BadRequest.ToString(), e.Message)
                    )
                )
            );
        }

        return Ok(
            new ApiResponse<TResult>(
                Result.Success(),
                result
            )
        );
    }

    protected async Task<IActionResult> ApiResponse(ICommand command)
    {
        try
        {
            await commandBus.SendAsync(command);
        }
        catch (System.Exception e)
        {
            return BadRequest(
                new ApiResponse<IActionResult>(
                    Result.Failure(
                        new Error(HttpStatusCode.BadRequest.ToString(), e.Message)
                    )
                )
            );
        }

        return Ok(
            new ApiResponse<IActionResult>(Result.Success())
        );
    }
}
