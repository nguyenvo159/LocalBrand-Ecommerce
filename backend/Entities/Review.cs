using System.ComponentModel.DataAnnotations;

namespace backend.Entity;

public record class Review
{
    [Key]
    public Guid Id { get; init; }

    [Required]
    public Guid ProductId { get; set; }

    public Product? Product { get; set; }

    [Required]
    public Guid UserId { get; set; }

    public User? User { get; set; }

    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    public int Rating { get; set; }

    [StringLength(1000, ErrorMessage = "Comment length can't be more than 1000.")]
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

}
