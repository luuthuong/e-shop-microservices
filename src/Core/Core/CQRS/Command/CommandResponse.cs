namespace Core.Infrastructure.CQRS;

public class CommandResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public Guid? Id { get; set; }
    public List<string> Errors { get; set; } = [];
}