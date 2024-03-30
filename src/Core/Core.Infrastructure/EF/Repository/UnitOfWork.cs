using System.Data;
using Core.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Infrastructure.EF.Repository;

public class UnitOfWork<TDbContext>(
    TDbContext dbContext
    ): IUnitOfWork<TDbContext> where TDbContext: IDbContext
{
    public ValueTask<int> SaveChangeAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangeAsync(cancellationToken);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Unspecified,
        CancellationToken cancellationToken = default)
    {
        IDbContextTransaction dbContextTransaction = await dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        return dbContextTransaction;
    }
}