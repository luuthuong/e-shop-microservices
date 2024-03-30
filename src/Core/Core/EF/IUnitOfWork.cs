using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.EF;

public interface IUnitOfWork<TDbContext> where TDbContext: IDbContext
{
    public ValueTask<int> SaveChangeAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.Unspecified,
        CancellationToken cancellationToken = default);
    
}