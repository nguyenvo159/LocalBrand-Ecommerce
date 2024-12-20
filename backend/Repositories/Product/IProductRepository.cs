﻿using backend.Entity;

namespace backend.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task<Product?> GetByNameAsync(string name);
    Task<List<Product>> GetByRate();
    Task<List<Product>> GetBestSell();

    Task<List<Product>> GetByCategoryAsync(Guid categoryId);
    Task<Product> CreateAsync(Product product);
    Task<Product?> UpdateAsync(Product product);
    Task<bool> DeleteAsync(Guid id);
    Task<List<Product>> SearchAsync(string search);

    IQueryable<Product> AsQueryable();

}
