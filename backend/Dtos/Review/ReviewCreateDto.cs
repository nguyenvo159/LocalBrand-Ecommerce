using System.ComponentModel.DataAnnotations;

namespace backend.Dto.Review;


public class ReviewCreateDto
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; } = 5;

    public string? Comment { get; set; } = string.Empty;

}
