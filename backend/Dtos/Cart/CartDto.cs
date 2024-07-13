namespace backend.Dto.Cart;

public class CartDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Total
    {
        get
        {
            return CartItems.Sum(item => item.ProductPrice * item.Quantity);
        }
    }
    public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
}
