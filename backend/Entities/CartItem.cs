namespace backend.Entity;

public record class CartItem
{
    public Guid Id { get; init; }
    public Guid CartId { get; set; }
    public virtual Cart? Cart { get; set; }

    public Guid ProductId { get; set; }
    public virtual Product? Product { get; set; }

    public Guid SizeId { get; set; }
    public virtual Size? Size { get; set; }

    public int Quantity { get; set; }

}
