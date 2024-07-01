using System.ComponentModel.DataAnnotations;

namespace backend.Entity;

public record class Size
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "Name length can't be more than 50.")]
    public string Name { get; set; } = string.Empty;

    public List<CartItem> CartItems { get; set; } = new List<CartItem>();

}
