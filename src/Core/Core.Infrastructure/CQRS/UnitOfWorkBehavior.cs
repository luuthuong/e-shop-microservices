using Core.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.CQRS;

public class UnitOfWorkBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse> where TRequest: notnull
{
    private readonly IDbContext _context;

    protected UnitOfWorkBehavior(IDbContext context)
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