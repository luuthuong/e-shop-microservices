namespace Core.Results;

public record PagedResult<T>(
    long Total,
    int PageSize,
    int PageIndex,
    IEnumerable<T> Data
);