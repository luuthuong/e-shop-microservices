using AutoMapper;
using Core.Mediator;
using Infrastructure.Database.Interfaces;
using MediatR;

namespace Application.Helpers;

public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> 
    where TRequest: IBaseRequest<TResponse>
{

    protected readonly IMapper Mapper;
    protected readonly IAppDbContext DBContext;

    protected BaseRequestHandler(IMapper mapper, IAppDbContext dbContext)
    {
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        DBContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}