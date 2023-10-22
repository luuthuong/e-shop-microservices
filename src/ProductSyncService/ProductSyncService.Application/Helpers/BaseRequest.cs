using AutoMapper;
using MediatR;
using ProductSyncService.Infrastructure.Database.Interfaces;

namespace ProductSyncService.Application.Helpers;

public abstract record BaseRequest<T> : IRequest<T>;

public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> 
    where TRequest: BaseRequest<TResponse>
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