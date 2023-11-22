namespace Core.Results;

public record PagedResult<T>(
    long Total,
    IEnumerable<T> Data
);