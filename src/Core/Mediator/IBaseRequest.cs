using MediatR;

namespace Core.Mediator;

public interface IBaseRequest<out TResponse> : IRequest<TResponse>
{
}