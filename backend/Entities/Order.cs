using System.ComponentModel.DataAnnotations;
using backend.Entity;

namespace backend;

public record class Order
{
    [Key]
    public Guid Id { get; init; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public User User { get; set; } = new User();

    [Range(0.01, double.MaxValue, ErrorMessage = "TotalAmount must be greater than zero.")]
    public decimal TotalAmount { get; set; }

    [Required]
    [StringLength(200, ErrorMessage = "Address length can't be more than 200.")]
    public string Address { get; set; } = string.Empty;

    [Required]
    [StringLength(50, ErrorMessage = "OrderStatus length can't be more than 50.")]
    public string OrderStatus { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
