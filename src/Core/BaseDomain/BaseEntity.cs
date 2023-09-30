using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.BaseDomain;

public abstract class BaseEntity: IEquatable<BaseEntity>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private init; }
    public DateTime CreatedDate { get; protected init; }
    public DateTime? UpdatedDate { get; protected set; }

    public static bool operator ==(BaseEntity? first, BaseEntity? second) =>
        first is not null && second is not null && first.Equals(second);

    public static bool operator !=(BaseEntity? first, BaseEntity? second) => !(first == second);

    public bool Equals(BaseEntity? other)
    {
        if (ReferenceEquals(null, other)) 
            return false;
        
        if (ReferenceEquals(this, other)) 
            return true;
        
        return Id.Equals(other.Id) 
               && CreatedDate.Equals(other.CreatedDate) 
               && Nullable.Equals(UpdatedDate, other.UpdatedDate);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((BaseEntity)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, CreatedDate, UpdatedDate);
    }
}