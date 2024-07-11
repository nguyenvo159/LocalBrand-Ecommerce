using backend.Data;
using backend.Dto.Review;
using backend.Entity;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class ReviewRepository : Repository<Review>, IReviewRepository
{
    public ReviewRepository(AppDbContext context) : base(context)
    {
    }


    public async Task<IEnumerable<Review>> GetReviewByProductIdAsync(Guid productId)
    {
        return await _dbSet.Where(r => r.ProductId == productId)
                            .Include(r => r.Product)
                            .Include(r => r.User)
                            .ToListAsync();
    }

    public async Task<Review?> GetReviewByIdAsync(Guid id)
    {
        return await _dbSet.Include(r => r.Product).Include(r => r.User)
                            .FirstOrDefaultAsync(r => r.Id == id);
    }
}

