using backend.Entity;

namespace backend;

public interface ISizeRepository
{
    Task<Size> GetByNameAsync(string name);

}
