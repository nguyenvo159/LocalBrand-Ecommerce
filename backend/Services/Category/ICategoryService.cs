using System;
using backend.Dtos.Category;

namespace backend.Services;

public interface ICategoryService
{
    Task<CategoryDto> Get(Guid id);
    Task<List<CategoryDto>> GetAll();
    Task Save(CategoryCreateUpdateDto category);
    Task Delete(Guid id);
}
