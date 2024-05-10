namespace Core.Domain;

public abstract class BaseEntity<TId> where TId: StronglyTypeId<Guid>
{
    public TId Id { get; protected set; }
    public DateTime CreatedDate { get; protected set; }
    public DateTime? UpdatedDate { get; protected set; }
    
    public bool IsDeleted { get; private set; }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }
}