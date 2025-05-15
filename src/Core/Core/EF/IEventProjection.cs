namespace Core.EF;

public interface IEventProjection
{
    Task ProjectAsync(CancellationToken cancellationToken = default(CancellationToken));
}