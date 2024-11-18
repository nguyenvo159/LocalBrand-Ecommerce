namespace backend.Entity;

public record class InventoryLog
{
    public Guid Id { get; set; }
    public Guid ProductInventoryId { get; set; }
    public virtual ProductInventory? ProductInventory { get; set; }
    public int Inventory { get; set; }
    public int OldInventory { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UpdatedBy { get; set; }
    public string ByName { get; set; } = string.Empty;
    public virtual User? User { get; set; }

}
