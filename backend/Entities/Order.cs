using backend.Entity;

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

    public string OrderStatus { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
