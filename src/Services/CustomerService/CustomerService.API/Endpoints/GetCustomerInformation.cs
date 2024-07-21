using Application.Queries;
using Core.Api;
using Core.Infrastructure.Api;

namespace API.Endpoints;

internal sealed class GetCustomerInformation
{
    internal sealed class Endpoint(IServiceScopeFactory serviceScopeFactory)
        : AbstractApiEndpoint(serviceScopeFactory), IApiEndpoint
    {
        public void Register(IEndpointRouteBuilder app)
        {
            app.MapGet("/customers/user-information", () => ApiResponse(
                    new GetCustomerLoginViaTokenQuery()
                )
            ).RequireAuthorization();
        }
    }
}