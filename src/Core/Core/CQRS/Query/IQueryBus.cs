namespace Core.CQRS.Query;

public interface IQueryBus
{
    Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> query);
}