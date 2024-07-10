using backend.Entity;

namespace backend;

public record class Order
{
    public Guid Id { get; init; }

    public Guid UserId { get; set; }

    public User User { get; set; } = new User();

    public decimal TotalAmount { get; set; }

    public string Address { get; set; } = string.Empty;

    public string OrderStatus { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
