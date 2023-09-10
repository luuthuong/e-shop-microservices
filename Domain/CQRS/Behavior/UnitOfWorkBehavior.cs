using Domain.Core;
using Domain.Database.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Domain.CQRS.Behavior;

public sealed class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : BaseRequest
{
    private readonly IAppDbContext _appDbContext;

    public UnitOfWorkBehavior(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!request.GetType().Name.EndsWith("Command"))
            return await next();
        if (!request.AutoSave)
            return await next();
        var strategy = _appDbContext.Database.CreateExecutionStrategy();
        return await strategy.Execute(async () =>
        {
            await using var transaction = await _appDbContext.Database.BeginTransactionAsync(cancellationToken);
            var response = await next();
            await _appDbContext.SaveChangeAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return response;
        });
    }
}
