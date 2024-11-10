namespace backend.Dto.Order;
using backend.Text.Enums;

public class OrderDto
{
    public Guid Id { get; init; }

    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string UserPhone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public Guid? DiscountId { get; set; }

    public decimal TotalAmount { get; set; }

    public Enums.OrderStatus Status { get; set; }
    public Enums.ShipType ShipType { get; set; }
    public Enums.PayType? PayType { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();

}
