using backend.Dto.Product;

namespace backend.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAll();
    Task<ProductDto> GetById(Guid id);
    Task<List<ProductDto>> GetByCategory(string categoryName);
    Task<ProductDto> Create(ProductCreateDto productDto);
    Task<ProductDto> Update(ProductUpdateDto productDto);
    Task<bool> Delete(Guid id);
}
