namespace Core.EF;

public interface IDbSeeder<TDbContext> where TDbContext: IDbContext
{
    string Key { get; }
    Task DoAsync(TDbContext dbContext, IServiceProvider services);
}