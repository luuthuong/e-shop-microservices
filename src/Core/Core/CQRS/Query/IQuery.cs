using MediatR;

namespace Core.CQRS.Query;

public interface IQuery<out TResponse>: IRequest<TResponse>
{
}