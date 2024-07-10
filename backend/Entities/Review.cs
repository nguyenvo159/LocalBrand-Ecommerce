
namespace backend.Entity;

public record class Review
{
    public Guid Id { get; init; }

    public Guid ProductId { get; set; }

    public Product? Product { get; set; }

    public Guid UserId { get; set; }

    public User? User { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

}
