using backend.Entity;

namespace backend.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task CreateUserAsync(User user);
    Task<bool> CheckPasswordAsync(User user, string password);
}
