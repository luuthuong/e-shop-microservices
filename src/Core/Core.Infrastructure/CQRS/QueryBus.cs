using Core.CQRS.Query;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.CQRS;

public class QueryBus: IQueryBus
{
    private readonly IMediator _mediator;
    private readonly ILogger<QueryBus> _logger;

    public QueryBus(IMediator mediator, ILogger<QueryBus> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> query)
    {
        _logger.LogInformation("Executing query: {query}", query);
        return _mediator.Send(query);
    }
}