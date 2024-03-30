namespace Core.Outbox;

public class OutboxMessage
{
    public Guid Id { get; init; }
    public string Type { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public DateTime ExecutedOnUtc { get; init; }
    public DateTime? ProcessedOnUtc { get; set; }
    public string ErrorMsg { get; set; } = string.Empty;
}