namespace Core.BaseDTO;

public record PagedResult<T>(
    long Total,
    IEnumerable<T> Data
);