namespace backend.Entity;

public record class CartItem
{
    public Guid Id { get; init; }
    public Guid CartId { get; set; }
    public Cart? Cart { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

    public Guid SizeId { get; set; }
    public Size? Size { get; set; }

    public int Quantity { get; set; }

}
