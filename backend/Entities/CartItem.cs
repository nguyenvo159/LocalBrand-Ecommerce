using System.ComponentModel.DataAnnotations;

namespace backend.Entity;

public record class CartItem
{
    [Key]
    public Guid Id { get; init; }
    [Required]
    public Guid CartId { get; set; }
    public Cart? Cart { get; set; }

    public Guid? ProductId { get; set; }
    public Product? Product { get; set; }

    public Guid? SizeId { get; set; }
    public Size? Size { get; set; }

    [Range(1, 999, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }

}
