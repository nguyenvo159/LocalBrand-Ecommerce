
namespace backend.Entity;

public record class ProductInventory
{
    public Guid Id { get; init; }

    public Guid ProductId { get; set; }

    public Product? Product { get; set; }

    public Guid SizeId { get; set; }

    public Size? Size { get; set; }

    public int Inventory { get; set; } = 999;
}
