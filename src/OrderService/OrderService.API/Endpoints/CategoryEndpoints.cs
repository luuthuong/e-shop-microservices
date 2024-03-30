using Core.Api;

namespace API.Endpoints;

internal sealed class CategoryEndpoints: IApiEndpoint
{
    public void Register(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/categories", () =>
        {
            return true;
        });
    }
}