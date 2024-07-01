using System.ComponentModel.DataAnnotations;

namespace backend.Entity;

public record class OrderItem
{
    [Key]
    public Guid Id { get; init; }

    [Required]
    public Guid OrderId { get; set; }

    public Order? Order { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    public Product? Product { get; set; }

    [Range(1, 999, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }

}
