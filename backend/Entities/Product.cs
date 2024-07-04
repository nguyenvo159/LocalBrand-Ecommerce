using System.ComponentModel.DataAnnotations;

namespace backend.Entity;

public record class Product
{
    [Key]
    public Guid Id { get; init; }

    [Required]
    [StringLength(100, ErrorMessage = "Name length can't be more than 100.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Description length can't be more than 1000.")]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public Guid CategoryId { get; set; }

    public Category Category { get; set; } = new Category();

    public List<ProductInventory> Sizes { get; set; } = new List<ProductInventory>();
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    public List<Review> Reviews { get; set; } = new List<Review>();
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
