using System.Linq.Expressions;
using backend.Entity;

namespace backend.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> DeleteIfAsync(Expression<Func<T, bool>> predicate);

    IQueryable<T> AsQueryable();
    //Special
    // Task<Cart?> GetCartAsync(Expression<Func<Cart, bool>> predicate);
    // Task<Order?> GetOrderAsync(Expression<Func<Order, bool>> predicate);
    // Task<List<Order>> GetAllOrderAsync(Expression<Func<Order, bool>> predicate);
}
