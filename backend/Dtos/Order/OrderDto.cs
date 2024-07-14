namespace backend.Dto.Order;

public class OrderDto
{
    public Guid Id { get; init; }

    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string UserPhone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }

    public string OrderStatus { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();

}
