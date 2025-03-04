using Core.CQRS.Query;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.CQRS;

public class QueryBus(IMediator mediator, ILogger<QueryBus> logger) : IQueryBus
{
    public Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> query)
    {
        logger.LogDebug("Executing query: {query}", query);
        return mediator.Send(query);
    }
}