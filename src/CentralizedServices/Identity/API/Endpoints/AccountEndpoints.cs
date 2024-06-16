using Core;
using Core.Api;
using Core.Identity;
using Identity.API.Requests;
using Identity.Domains;

namespace Identity.API.Endpoints;

internal sealed class AccountEndpoints : IApiEndpoint
{
    public void Register(IEndpointRouteBuilder app)
    {
        app.MapPost("/accounts/register", RegisterAccount);

        app.MapPost("accounts/login", LoginAccount);

        app.MapPost("accounts/logout", () => { });
    }

    private async Task<IResult> RegisterAccount(IIdentityManager identityManager, RegisterUserRequest request)
    {
        try
        {
            var result = await identityManager.RegisterNewUser(request);

            return TypedResults.Ok(new
            {
                data = result,
                success = result.Succeeded
            });
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }

    private async Task<IResult> LoginAccount(IIdentityManager identityManager, LoginRequest request)
    {
        try
        {
            var response = await identityManager.AuthUserByCredentials(request);

            return TypedResults.Ok(
                new UserLoginResponse(
                    response.AccessToken!,
                    response.RefreshToken!,
                    response.Scope!,
                    new(
                        response.ErrorType,
                        response.ErrorDescription,
                        response.HttpErrorReason
                    )
                )
            );
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }
}