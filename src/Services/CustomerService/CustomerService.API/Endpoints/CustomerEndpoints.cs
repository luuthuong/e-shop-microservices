using API.Requests;
using Application.Commands;
using Application.Queries;
using Core.Api;
using Core.Identity;
using Core.Infrastructure.Api;

namespace API.Endpoints;

public sealed class CustomerEndpoints(IServiceScopeFactory serviceScopeFactory)
    : AbstractApiEndpoint(serviceScopeFactory), IApiEndpoint
{
    public void Register(IEndpointRouteBuilder app)
    {
        app.MapPost("/customers", CreateCustomer);
        
        app.MapGet("/customers/user-information", GetUserInformation)
            .RequireAuthorization();
        
        app.MapPut("/customers/deactivate{id}", DeactivateCustomer)
            .RequireAuthorization(AuthPolicyBuilder.Admin);
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

    private Task<IResult> DeactivateCustomer()
    {
        return default;
    }
}