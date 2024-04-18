using Core.Api;
using Core.Infrastructure.Api;

namespace API.Endpoints;

public class CategoryEndpoints(IServiceScopeFactory serviceScopeFactory) : AbstractApiEndpoint(serviceScopeFactory), IApiEndpoint
{
    public void Register(IEndpointRouteBuilder app)
    {
    }
}