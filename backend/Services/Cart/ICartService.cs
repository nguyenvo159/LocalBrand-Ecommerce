using backend.Dto.Cart;

namespace backend.Services;

public interface ICartService
{
    Task<CartDto?> GetById(Guid id);
    Task<CartDto?> GetByUserId(Guid userId);

    Task<CartDto> AddToCart(CartItemCreateDto cartItemCreateDto);
    Task<CartItemDto> UpdateCartItem(CartItemUpdateDto cartItemUpdateDto);
    Task RemoveItem(Guid cartItemId);
    Task ClearCart(Guid id);

}
