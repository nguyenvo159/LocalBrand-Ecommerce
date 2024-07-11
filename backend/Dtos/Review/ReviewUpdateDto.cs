using System.ComponentModel.DataAnnotations;

namespace backend.Dto.Review;

public class ReviewUpdateDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    public string? Comment { get; set; } = string.Empty;
}
