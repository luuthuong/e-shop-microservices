using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Database;

public class Repository<T> where T: BaseEntity
{
    private readonly DbSet<T> _dbSet;
    private readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async ValueTask<T> Insert(T entity, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        if (autoSave)
            await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<T> Update(T entity, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        var entry = _dbSet.Entry(entity);
        entry.State = EntityState.Modified;
        if (autoSave)
            await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async ValueTask<bool> Delete(T entity, bool autoSave, CancellationToken cancellationToken = default)
    {
        bool isSuccess = false;
        _dbSet.Remove(entity);
        if (autoSave)
            isSuccess = await _context.SaveChangesAsync(cancellationToken) > 0;
        return isSuccess;
    }

    public Task<T> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbSet.Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }
}