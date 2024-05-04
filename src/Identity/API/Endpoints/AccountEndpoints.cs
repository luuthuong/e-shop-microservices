using Core.Api;
using Identity.API.Requests;
using Identity.Domains;

namespace Identity.API.Endpoints;

public class AccountEndpoints: IApiEndpoint
{
    public void Register(IEndpointRouteBuilder app)
    {
        app.MapPost("/accounts/register", 
            async (IIdentityManager identityManager, RegisterUserRequest request) =>
        {
            try
            {
                var result = await identityManager.RegisterNewUser(request);

                return Results.Ok(new
                {
                    data = result,
                    success = result.Succeeded
                });
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);            
            }
        });

        app.MapPost("accounts/login",
            async (IIdentityManager identityManager, LoginRequest request) =>
            {
                try
                {
                    var response = await identityManager
                        .AuthUserByCredentials(request);

                    return Results.Ok(response);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }
            }
        );
    }
}