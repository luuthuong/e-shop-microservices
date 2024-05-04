using MediatR;

namespace Core.CQRS.Command;

public interface ICommand : IRequest;

public interface ICommand<out TResponse>: IRequest<TResponse> where TResponse: notnull;