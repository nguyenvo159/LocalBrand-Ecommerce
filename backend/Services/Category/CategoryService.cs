using System;
using AutoMapper;
using backend.Dtos.Category;
using backend.Entity;
using backend.Repositories;

namespace backend.Services;

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly IMapper _mapper;
    public CategoryService(IRepository<Category> categoryRepository, IMapper mapaper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapaper;
    }
    public async Task Delete(Guid id)
    {
        await _categoryRepository.DeleteAsync(id);
    }

    public async Task<CategoryDto> Get(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            throw new ApplicationException("Category not found");
        }
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<List<CategoryDto>> GetAll()
    {
        var categories = await _categoryRepository.GetAllAsync();
        if (categories == null)
        {
            throw new ApplicationException("No categories found");
        }
        return _mapper.Map<List<CategoryDto>>(categories);
    }

    public async Task Save(CategoryCreateUpdateDto category)
    {
        if (category.Id != null)
        {
            await Update(category);
        }
        else
        {
            await Create(category);
        }
    }

    private async Task Update(CategoryCreateUpdateDto category)
    {
        var categoryEntity = await _categoryRepository.GetByIdAsync(category.Id!.Value);
        if (categoryEntity == null)
        {
            throw new ApplicationException("Category not found");
        }
        categoryEntity.Name = category.Name;
        await _categoryRepository.UpdateAsync(categoryEntity);
    }

    private async Task Create(CategoryCreateUpdateDto category)
    {
        var categoryEntity = new Category
        {
            Name = category.Name
        };
        await _categoryRepository.AddAsync(categoryEntity);
    }
}
