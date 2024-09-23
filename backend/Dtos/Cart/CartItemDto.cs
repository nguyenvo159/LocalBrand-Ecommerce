namespace backend.Dto.Cart;

public class CartItemDto
{
    public Guid Id { get; init; }
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal ProductPrice { get; set; }
    public string? ProductImg { get; set; }
    public Guid SizeId { get; set; }
    public string? SizeName { get; set; }
    public int Quantity { get; set; }

}
