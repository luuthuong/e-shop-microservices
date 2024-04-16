using System.Net;
using Core.Api;
using Core.CQRS.Command;
using Core.CQRS.Query;
using Core.Results;
using Microsoft.AspNetCore.Http;
using HttpResults = Microsoft.AspNetCore.Http.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Api;

public abstract class AbstractApiEndpoint(IServiceScopeFactory serviceScopeFactory)
{
    protected async Task<IResult> ApiResponse<TResult>(IQuery<TResult> query)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var queryBus = scope.ServiceProvider.GetRequiredService<IQueryBus>();
        TResult result;
        try
        {
            result = await queryBus.SendAsync(query);
        }
        catch (System.Exception e)
        {
            return HttpResults.BadRequest(
                new ApiResponse<TResult>(
                    Result.Failure(
                        new Error(HttpStatusCode.BadRequest.ToString(), e.Message)
                    )
                )
            );
        }

        return HttpResults.Ok(
            new ApiResponse<TResult>(
                Result.Success(),
                result
            )
        );
    }

    protected async Task<IResult> ApiResponse(ICommand command)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var commandBus = scope.ServiceProvider.GetRequiredService<ICommandBus>();
        try
        {
            await commandBus.SendAsync(command);
        }
        catch (System.Exception e)
        {
            return HttpResults.BadRequest(
                new ApiResponse(
                    Result.Failure(
                        new Error(HttpStatusCode.BadRequest.ToString(), e.Message)
                    )
                )
            );
        }

        return HttpResults.Ok(
            new ApiResponse(Result.Success())
        );
    }
}