using System.ComponentModel.DataAnnotations;

namespace backend.Entity;

public record class Cart
{
    [Key]
    public Guid Id { get; init; }
    [Required]
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
}
