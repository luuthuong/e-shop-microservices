using API.Requests;
using Application.Commands;
using Application.Queries;
using Core.Api;
using Core.Identity;
using Core.Infrastructure.Api;

namespace API.Endpoints;

public class CustomerEndpoints(IServiceScopeFactory serviceScopeFactory)
    : AbstractApiEndpoint(serviceScopeFactory), IApiEndpoint
{
    public void Register(IEndpointRouteBuilder app)
    {
        app.MapPost("/customers", CreateCustomer).RequireAuthorization(AuthPolicyBuilder.CanWrite);
        app.MapGet("/customers/user-information", GetUserInformation).RequireAuthorization();
    }

    private Task<IResult> CreateCustomer(CreateCustomerRequest request) => ApiResponse(
        new CreateCustomerCommand(
            request.Email,
            request.PasswordConfirm,
            request.Password,
            request.Name,
            request.ShippingAddress,
            request.CreditLimit
        )
    );

    private Task<IResult> GetUserInformation() => ApiResponse(
        new GetCustomerLoginViaTokenQuery()
    );
}