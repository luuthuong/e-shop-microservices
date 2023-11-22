using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.EF;

public class SeedingHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required] 
    public string Key { get; set; } = string.Empty;
    
    [Required]
    public string EntityName { get; set; } = string.Empty;
}