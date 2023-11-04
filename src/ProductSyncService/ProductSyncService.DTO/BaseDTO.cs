namespace ProductSyncService.Application.DTO;

public abstract class BaseDTO
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public record PageRequest(int PageSize, int PageIndex);

public record PageResponse<T>
{
    public long Total { get; init; }
    public long TotalPage { get; init; }
    public IEnumerable<T> Data { get; init; }
}