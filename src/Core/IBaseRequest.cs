using MediatR;

namespace Domain.Core;

public interface IBaseRequest<out TResponse> : IRequest<TResponse>
{
}