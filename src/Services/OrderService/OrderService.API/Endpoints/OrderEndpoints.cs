using Core.Api;
using Core.Infrastructure.Api;

namespace API.Endpoints;

internal sealed class OrderEndpoints(IServiceScopeFactory serviceScopeFactory) : AbstractApiEndpoint(serviceScopeFactory), IApiEndpoint
{
    public void Register(IEndpointRouteBuilder app)
    {
    }
}