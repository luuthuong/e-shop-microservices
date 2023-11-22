namespace Core.Domain;

public abstract class BaseEntity
{
    public DateTime CreatedDate { get; protected init; }
    public DateTime? UpdatedDate { get; protected set; }
    
    public bool IsDeleted { get; private set; }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }
}