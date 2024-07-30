
namespace backend.Entity;

public record class Review
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public virtual Product? Product { get; set; }

    public Guid UserId { get; set; }

    public virtual User? User { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

}
