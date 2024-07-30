
namespace backend.Entity;

public record class ProductImage
{
    public Guid Id { get; init; }

    public Guid ProductId { get; set; }
    public virtual Product? Product { get; set; }

    public string? ImageUrl { get; set; }
    public float[]? ImageVector { get; set; }

}
