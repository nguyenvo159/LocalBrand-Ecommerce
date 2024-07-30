
namespace backend.Entity;

public record class OrderItem
{
    public Guid Id { get; init; }

    public Guid OrderId { get; set; }

    public virtual Order? Order { get; set; }

    public Guid? ProductId { get; set; }

    public virtual Product? Product { get; set; }

    public Guid SizeId { get; set; }

    public virtual Size? Size { get; set; }

    public int Quantity { get; set; }

}
