using System.ComponentModel.DataAnnotations;

namespace backend.Entity;

public record class ProductImage
{
    [Key]
    public Guid Id { get; init; }

    [Required]
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

    [Required]
    [Url(ErrorMessage = "Invalid Image URL format.")]
    public string? ImageUrl { get; set; }
    public float[]? ImageVector { get; set; }

}
