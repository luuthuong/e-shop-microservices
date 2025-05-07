namespace Core.Domain;

public class Snapshot
{
    public Guid AggregateId { get; set; }
    public string AggregateType { get; set; }
    public int Version { get; set; }
    public string State { get; set; }
    public DateTime Timestamp { get; set; }
}