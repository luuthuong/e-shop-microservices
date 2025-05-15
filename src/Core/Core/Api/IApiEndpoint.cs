using Microsoft.AspNetCore.Routing;

namespace Core.Api;

public interface IApiEndpoint
{
    void Register(IEndpointRouteBuilder route);
    
    string GroupName { get; }
}