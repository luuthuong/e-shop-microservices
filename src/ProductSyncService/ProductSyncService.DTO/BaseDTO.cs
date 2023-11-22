namespace ProductSyncService.DTO;

public abstract class BaseDTO
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}