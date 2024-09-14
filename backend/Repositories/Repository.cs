
using System.Linq.Expressions;
using backend.Data;
using backend.Entity;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }


    public Task<List<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
    {
        var result = _dbSet.Where(predicate).ToListAsync();
        return result;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }


    public async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> DeleteIfAsync(Expression<Func<T, bool>> predicate)
    {
        var entities = await _dbSet.Where(predicate).ToListAsync();
        if (!entities.Any())
        {
            return false;
        }

        _dbSet.RemoveRange(entities);
        await _context.SaveChangesAsync();
        return true;
    }

    public IQueryable<T> AsQueryable()
    {
        return _context.Set<T>().AsQueryable();
    }

    public async Task<bool> DeleteListAsync(List<T> entities)
    {
        if (!entities.Any())
        {
            return false;
        }

        _dbSet.RemoveRange(entities);
        await _context.SaveChangesAsync();
        return true;
    }
}
