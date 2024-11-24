
namespace backend.Entity;

public record class OrderItem
{
    public Guid Id { get; init; }

    public Guid OrderId { get; set; }

    public virtual Order? Order { get; set; }

    public Guid? ProductId { get; set; }

    public virtual Product? Product { get; set; }
    public string ProductName { get; set; }
    public decimal ProductPrice { get; set; }
    public string ProductImg { get; set; }

    public Guid SizeId { get; set; }

    public virtual Size? Size { get; set; }
    public string SizeName { get; set; }

    public int Quantity { get; set; }

}
