
namespace backend.Entity;

public record class Size
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public virtual List<CartItem> CartItems { get; set; } = new List<CartItem>();

}
