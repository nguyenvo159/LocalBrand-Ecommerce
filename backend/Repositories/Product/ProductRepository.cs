using backend.Data;
using backend.Entity;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
    }
    public async Task<List<Product>> GetByCategoryAsync(Guid categoryId)
    {
        return await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }
    public async Task<Product?> GetByNameAsync(string name)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Name == name);
    }


    public async Task<Product> CreateAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }
    public async Task<Product?> UpdateAsync(Product product)
    {
        var existingProduct = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Sizes)
                .ThenInclude(s => s.Size)
            .Include(p => p.ProductImages)
            .FirstOrDefaultAsync(p => p.Id == product.Id);
        if (existingProduct == null)
        {
            return null;
        }

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.CategoryId = product.CategoryId;
        existingProduct.UpdatedAt = DateTime.UtcNow;
        // Update Sizes
        _context.ProductInventories.RemoveRange(existingProduct.Sizes);
        foreach (var size in product.Sizes)
        {
            existingProduct.Sizes.Add(size);
        }

        _context.Products.Update(existingProduct);
        await _context.SaveChangesAsync();
        return existingProduct;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<List<Product>> SearchAsync(string search)
    {
        var keywords = search.ToLower().Split(' ');
        var products = await _context.Products
            .Where(p => keywords.Any(keyword =>
                p.Name.ToLower().Contains(keyword) ||
                p.Description.ToLower().Contains(keyword) ||
                p.Category.Name.ToLower() == keyword))
            .ToListAsync();
        return products;
    }

    public async Task<List<Product>> GetByRate()
    {
        var date = DateTime.UtcNow.AddDays(-30);
        return await _context.Products
            .Include(p => p.Reviews)
            .Where(p => p.Reviews.Any(r => r.CreatedAt > date))
            .OrderByDescending(p => p.Reviews.Average(r => r.Rating))
            .Take(10)
            .ToListAsync();
    }

    public async Task<List<Product>> GetBestSell()
    {
        var date = DateTime.UtcNow.AddDays(-30);

        return await _context.Products
        .Include(a => a.OrderItems)
            .Where(p => p.OrderItems.Any(oi => oi.Order.CreatedAt > date))
            .OrderByDescending(p => p.OrderItems
                .Where(oi => oi.Order.CreatedAt > date)
                .Sum(oi => oi.Quantity))
            .Take(10)
            .ToListAsync();
    }

    public IQueryable<Product> AsQueryable()
    {
        return _context.Products.AsQueryable();
    }
}
