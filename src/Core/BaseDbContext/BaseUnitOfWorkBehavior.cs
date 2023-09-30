using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.BaseDbContext;

public abstract class BaseUnitOfWorkBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse> where TRequest: notnull
{
    private readonly IBaseDbContext _context;

    protected BaseUnitOfWorkBehavior(IBaseDbContext context)
    {
        _context = context;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!request.GetType().Name.EndsWith("Command"))
            return await next();
        var strategy = _context.Database.CreateExecutionStrategy();
        return await strategy.Execute(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            var response = await next();
            await _context.SaveChangeAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return response;
        });
    }
}