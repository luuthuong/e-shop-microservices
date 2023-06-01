namespace Domain.Entities;

public class Category: BaseEntity
{
    public string Name { get; set; }

    private Category(string name)
    {
        Name = name;
        CreatedDate = DateTime.Now;
    }

    public static Category Create(string name)
    {
        return new Category(name);
    }
}