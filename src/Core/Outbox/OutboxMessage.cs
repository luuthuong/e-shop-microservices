namespace Core.Outbox;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime ExecutedOnUtc { get; set; }
    public DateTime? ProcessedOnUtc { get; set; }
    public string ErrorMsg { get; set; } = string.Empty;
}