
namespace backend.Entity;

public record class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual Cart Cart { get; set; } = new Cart();
    public virtual List<Order> Orders { get; set; } = new List<Order>();
    public virtual List<Review> Reviews { get; set; } = new List<Review>();
}
