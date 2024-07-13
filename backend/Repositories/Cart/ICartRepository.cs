using backend.Entity;

namespace backend;

public interface ICartRepository
{
    Task<Cart> GetCartByIdAsync(Guid id);
    Task<Cart> GetCartByUserIdAsync(Guid id);
    Task ClearCart(Guid id);

}
