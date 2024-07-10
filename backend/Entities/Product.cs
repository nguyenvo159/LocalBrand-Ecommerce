
namespace backend.Entity;

public record class Product
{
    public Guid Id { get; init; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Guid CategoryId { get; set; }

    public Category? Category { get; set; }

    public List<ProductInventory> Sizes { get; set; } = new List<ProductInventory>();
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    public List<Review> Reviews { get; set; } = new List<Review>();
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
