using MediatR;

namespace Core.CQRS.Query;

public interface IQueryHandler<in TQuery, TResponse>: IRequestHandler<TQuery, TResponse> where TQuery: IQuery<TResponse>
{
}