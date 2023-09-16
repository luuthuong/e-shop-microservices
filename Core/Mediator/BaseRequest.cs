using MediatR;

namespace Core.Mediator;

public abstract record BaseRequest<T> : IRequest<BaseResponse<T>>;

public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, BaseResponse<TResponse>> 
    where TRequest : BaseRequest<TResponse> where TResponse: notnull
{
    public abstract Task<BaseResponse<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
}