using backend.Data;
using backend.Entity;

namespace backend;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Cart> GetCartByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Cart> GetCartByUserIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task ClearCart(Guid id)
    {
        throw new NotImplementedException();
    }
}
