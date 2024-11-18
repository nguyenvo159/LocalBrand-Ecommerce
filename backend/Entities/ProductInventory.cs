
namespace backend.Entity;

public record class ProductInventory
{
    public Guid Id { get; init; }

    public Guid ProductId { get; set; }

    public virtual Product? Product { get; set; }

    public Guid SizeId { get; set; }

    public virtual Size? Size { get; set; }

    public int Inventory { get; set; } = 999;

    public virtual List<InventoryLog> InventoryLogs { get; set; } = new List<InventoryLog>();
}
