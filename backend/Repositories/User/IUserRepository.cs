using backend.Entity;

namespace backend.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task CreateUserAsync(User user);
    Task<bool> CheckPasswordAsync(User user, string password);
    Task UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(Guid id);

}
