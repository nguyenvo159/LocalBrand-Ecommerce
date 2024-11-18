using System;

namespace backend.Dtos.InventoryLog;

public class InventoryLogDto
{
    public Guid Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string SizeName { get; set; } = string.Empty;
    public int Inventory { get; set; }
    public int OldInventory { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UpdatedBy { get; set; }
    public string ByName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;


}
