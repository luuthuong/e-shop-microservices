using Core.Api;
using Core.Infrastructure.Api;
using Domain;

namespace API.Endpoints;

internal sealed class OrderEndpoints(IServiceScopeFactory serviceScopeFactory) : AbstractApiEndpoint(serviceScopeFactory), IApiEndpoint
{
    public void Register(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", (IOrderRepository repository) =>
        {
            return repository.GetPagingResultAsync(10, 0, o => true);
        });
    }
}