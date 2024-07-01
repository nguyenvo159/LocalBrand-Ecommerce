using System.ComponentModel.DataAnnotations;

namespace backend.Entity;

public record class Category
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Name length can't be more than 100.")]
    public string Name { get; set; } = string.Empty;

    public List<Product> Products { get; set; } = new List<Product>();
}
