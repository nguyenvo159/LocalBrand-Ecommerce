using System.ComponentModel.DataAnnotations;

namespace backend.Dto.Cart;

public class CartItemUpdateDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid ProductId { get; set; }
    [Required]
    public Guid SizeId { get; set; }
    [Range(1, 99, ErrorMessage = "Quantity must be 1 to 999")]
    public int Quantity { get; set; }
}

