namespace backend;

public class OrderItemDto
{
    public Guid Id { get; init; }

    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }
    public Guid SizeId { get; set; }
    public string SizeName { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
