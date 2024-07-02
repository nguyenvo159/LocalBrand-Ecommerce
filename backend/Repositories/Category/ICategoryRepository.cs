using backend.Entity;

namespace backend.Repositories;

public interface ICategoryRepository
{
    Task<Category> GetByIdAsync(Guid id);

}
