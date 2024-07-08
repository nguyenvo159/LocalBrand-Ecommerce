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
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Sizes)
                .ThenInclude(s => s.Size)
            .Include(p => p.ProductImages)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Sizes)
                .ThenInclude(s => s.Size)
            .Include(p => p.ProductImages)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    public async Task<List<Product>> GetByCategoryAsync(Guid categoryId)
    {
        return await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .Include(p => p.Category)
            .Include(p => p.Sizes)
                .ThenInclude(s => s.Size)
            .Include(p => p.ProductImages)
            .ToListAsync();
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
        // Update Sizes
        _context.ProductInventories.RemoveRange(existingProduct.Sizes);
        foreach (var size in product.Sizes)
        {
            existingProduct.Sizes.Add(size);
        }

        // Update Images
        _context.ProductImages.RemoveRange(existingProduct.ProductImages);
        foreach (var image in product.ProductImages)
        {
            existingProduct.ProductImages.Add(image);
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
}
