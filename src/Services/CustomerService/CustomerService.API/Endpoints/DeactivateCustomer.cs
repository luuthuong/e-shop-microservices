using Core.Api;
using Core.Identity;
using Core.Infrastructure.Api;

namespace API.Endpoints;

internal sealed class DeactivateCustomer
{
    internal sealed class Endpoint(IServiceScopeFactory serviceScopeFactory)
        : AbstractApiEndpoint(serviceScopeFactory), IApiEndpoint
    {
        public override void Register(IEndpointRouteBuilder app)
        {
            app.MapPut("/customers/deactivate{id}", () => true).RequireAuthorization(AuthPolicyBuilder.Admin);
        }
    }
}