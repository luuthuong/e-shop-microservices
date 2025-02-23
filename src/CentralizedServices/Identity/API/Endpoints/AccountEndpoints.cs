using Core.Api;
using Core.Infrastructure.Api;
using Identity.API.Requests;
using Identity.Domains;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Endpoints;

internal sealed class AccountEndpoints(IServiceScopeFactory serviceScopeFactory) : AbstractApiEndpoint(serviceScopeFactory)
{
    public override void Register(IEndpointRouteBuilder app)
    {
        app.MapPost("/accounts/register", RegisterAccount);

        app.MapPost("accounts/login", LoginAccount);
    }

    private async Task<IResult> RegisterAccount(IIdentityManager identityManager, [FromBody]RegisterUserRequest request)
    {
        try
        {
            var result = await identityManager.RegisterNewUser(request);
        
            if (!result.Succeeded)
                return TypedResults.BadRequest(result.Errors.First().Description);
        
            return TypedResults.Ok(result);
        }
        catch(Exception e)
        {
            return TypedResults.BadRequest(
                IdentityResult.Failed(
                    new IdentityError()
                    {
                        Code = "identity.register-account-failed",
                        Description = e.Message
                    }
                )
            );
        }
       
    }

    private async Task<IResult> LoginAccount(IIdentityManager identityManager, LoginRequest request)
    {
        var result = await identityManager.AuthUserByCredentials(request);

        if (result.IsFailure)
            return TypedResults.BadRequest(result.Error.Description);

        var data = result.Data!;
        
        return TypedResults.Ok(
            new UserLoginResponse(
                data.AccessToken!,
                data.RefreshToken!,
                data.Scope!,
                new(
                    data.ErrorType,
                    data.ErrorDescription,
                    data.HttpErrorReason
                )
            )
        );
    }
}