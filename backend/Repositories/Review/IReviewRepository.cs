using backend.Entity;

namespace backend.Repositories;

public interface IReviewRepository : IRepository<Review>
{
    Task<Review?> GetReviewByIdAsync(Guid id);
    Task<IEnumerable<Review>> GetReviewByProductIdAsync(Guid productId);
}