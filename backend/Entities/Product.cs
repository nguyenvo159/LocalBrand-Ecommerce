
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

    public virtual Category? Category { get; set; }

    public virtual List<ProductInventory> Sizes { get; set; } = new List<ProductInventory>();
    public virtual List<CartItem> CartItems { get; set; } = new List<CartItem>();
    public virtual List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    public virtual List<Review> Reviews { get; set; } = new List<Review>();
    public virtual List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
