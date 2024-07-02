using backend.Dto.Product;

namespace backend.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAll();
    Task<ProductDto> GetById(Guid id);
    Task<ProductDto> Create(ProductCreateDto productDto);
    Task<bool> Delete(Guid id);
}
