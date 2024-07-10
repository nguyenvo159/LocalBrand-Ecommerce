namespace backend.Entity;

public record class Cart
{
    public Guid Id { get; init; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
}
