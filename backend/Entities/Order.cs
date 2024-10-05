using backend.Entity;
using backend.Text.Enums;

namespace backend;

public record class Order
{
    public Guid Id { get; init; }

    public Guid UserId { get; set; }

    public virtual User? User { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string UserPhone { get; set; } = string.Empty;

    public string UserEmail { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }

    public string Address { get; set; } = string.Empty;

    public string Note { get; set; } = string.Empty;

    public Enums.OrderStatus Status { get; set; } = Enums.OrderStatus.Pending;
    public Enums.ShipType ShipType { get; set; } = Enums.ShipType.Standard;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public Guid? DiscountId { get; set; }
    public virtual Discount? Discount { get; set; }
    public bool IsActived { get; set; } = true;
    public virtual List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
