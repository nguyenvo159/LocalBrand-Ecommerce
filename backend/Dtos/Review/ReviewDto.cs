namespace backend.Dto.Review;

public class ReviewDto
{
    public Guid Id { get; init; }

    public Guid ProductId { get; set; }
    public string? ProductName { get; set; }

    public Guid UserId { get; set; }

    public string? UserName { get; set; }
    public int Rating { get; set; }

    public string Comment { get; set; } = string.Empty;
}
