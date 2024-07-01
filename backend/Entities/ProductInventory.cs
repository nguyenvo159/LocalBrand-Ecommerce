using System.ComponentModel.DataAnnotations;

namespace backend.Entity;

public record class ProductInventory
{
    [Key]
    public Guid Id { get; init; }

    [Required]
    public Guid ProductId { get; set; }

    public Product? Product { get; set; }

    [Required]
    public Guid SizeId { get; set; }

    public Size? Size { get; set; }

    [Range(0, 999, ErrorMessage = "Inventory must be a non-negative number.")]
    public int Inventory { get; set; } = 999;
}
